using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json;
using NHibernate;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.Simulador;
using Tenda.EmpresaVenda.Domain.Integration;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS10")]
    public class PrePropostaController : BaseController
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private TipoDocumentoRepository _tipoDocumentoRepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private ViewPlanoPagamentoRepository _viewPlanoPagamentoRepository { get; set; }
        private ViewProponenteRepository _viewProponenteRepository { get; set; }
        private ViewDocumentoProponenteRepository _viewDocumentoProponenteRepository { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        private PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        private ProponenteRepository _proponenteRepository { get; set; }
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        private EnderecoEmpresaRepository _enderecoEmpresaRepository { get; set; }
        private FamiliarRepository _familiarRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private DocumentoProponenteService _documentoProponenteService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private EmpresaVendaController _empresaVendaController { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }

        private ViewHistoricoPrePropostaRepository _viewHistoricoPrePropostaRepository { get; set; }

        private DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private PlanoPagamentoService _planoPagamentoService { get; set; }
        private BoletoPrePropostaRepository _boletoPrePropostaRepository { get; set; }
        private ContratoPrePropostaRepository _contratoPrePropostaRepository { get; set; }
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private BreveLancamentoService _breveLancamentoService { get; set; }
        private PrePropostaReintegradaService _prePropostaReintegradaService { get; set; }
        private PrePropostaReintegradaRepository _prePropostaReintegradaRepository { get; set; }
        private DocumentoFormularioRepository _documentoFormularioRepository { get; set; }
        private HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }

        private AvalistaRepository _avalistaRepository { get; set; }
        private ViewDocumentoAvalistaRepository _viewDocumentoAvalistaRepository { get; set; }
        private AvalistaService _avalistaService { get; set; }
        private EnderecoAvalistaRepository _enderecoAvalistaRepository { get; set; }
        private EnderecoAvalistaService _enderecoAvalistaService { get; set; }
        private DocumentoAvalistaRepository _documentoAvalistaRepository { get; set; }
        private PropostaSuatRepository _propostaSuatRepository { get; set; }
        private DocumentoAvalistaService _documentoAvalistaService { get; set; }
        private TipoDocumentoAvalistaRepository _tipoDocumentoAvalistaRepository { get; set; }
        private RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        private DocumentoRuleMachinePrePropostaRepository _documentoRuleMachinePrePropostaRepository { get; set; }

        #region PreProposta
        [BaseAuthorize("EVS10", "Visualizar")]
        public ActionResult Index(long? id)
        {
            PreProposta preProposta = null;
            PropostaSuat proposta = new PropostaSuat();
            if (id.HasValue)
            {
                CheckAccess(id.Value);
                preProposta = _prePropostaRepository.FindById(id.Value);
                proposta = _propostaSuatRepository.FindByIdPreProposta(preProposta.Id);
            }

            PrePropostaDTO dto = new PrePropostaDTO();

            if (preProposta == null)
            {
                // Elaboração, Status e outros só são definidos ao salvar
                preProposta = new PreProposta();
                preProposta.Corretor = new Corretor();
                preProposta.Elaborador = new Corretor();
                preProposta.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();
                dto.ListProponentes = new List<SelectListItem>();
            }
            else
            {
                dto.ListProponentes = ProponentesPreProposta(preProposta.Id);
                dto.SituacaoPrepropostaSuatEvs = preProposta.SituacaoProposta.Value==SituacaoProposta.Integrada? 
                    _viewPrePropostaRepository.Queryable().Where(x=>preProposta.IdSuat==x.IdSuat || x.IdSuat == null)
                                                  .Where(x => x.Id == id.Value).Select(reg => reg.SituacaoPrePropostaSuatEvs).SingleOrDefault():preProposta.SituacaoProposta.AsString();
                dto.PossuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

                var historicoAnterior = _historicoPrePropostaRepository.HistoricoAnterior(preProposta.Id);

                if (historicoAnterior.HasValue())
                {
                    dto.SituacaoAnterior = historicoAnterior.SituacaoInicio;
                }
            }

            dto.PreProposta = preProposta;
            if (preProposta.BreveLancamento != null)
            {
                var enderecoBreveLancamento = _enderecoBreveLancamentoRepository.FindByBreveLancamento(preProposta.BreveLancamento.Id);
                dto.EnderecoBreveLancamento = enderecoBreveLancamento.ChaveCandidata();
            }


            // Lista os pontos de venda da Empresa de Venda do usuário logado
            dto.ListPontosVenda = _pontoVendaRepository.Queryable().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nome
            });

            // Lista os Breves Lançamentos da Empresa de Venda do usuário logado
            dto.ListBrevesLancamentos = _enderecoBreveLancamentoRepository.Queryable().Select(x => new SelectListItem
            {
                Value = x.BreveLancamento.Id.ToString(),
                Text = x.BreveLancamento.Nome
            });

            if (!proposta.IsEmpty())
            {
                dto.KitCompleto = proposta.KitCompleto;
                dto.PermissaoAvalistaPreChaves = _planoPagamentoRepository.RegraAvalistaPreChaves(preProposta.Id);
            }

            dto.AvalistaDTO = new AvalistaDTO();
            if (preProposta.Avalista != null)
            {
                var avalista = _avalistaRepository.FindById(preProposta.Avalista.Id);
                dto.AvalistaDTO.Avalista = avalista;

                var enderecoAvalista = _enderecoAvalistaRepository.FindByIdAvalista(avalista.Id);
                dto.AvalistaDTO.Endereco = enderecoAvalista;

                var situacao = new List<SituacaoAprovacaoDocumentoAvalista> {
                    SituacaoAprovacaoDocumentoAvalista.Anexado,
                    SituacaoAprovacaoDocumentoAvalista.Informado,
                    SituacaoAprovacaoDocumentoAvalista.NaoAnexado,
                    SituacaoAprovacaoDocumentoAvalista.Pendente
                };

                var docsPendentes = _documentoAvalistaRepository.Queryable()
                    .Where(x => x.Avalista.Id == avalista.Id)
                    .Where(x => situacao.Contains(x.Situacao))
                    .Any();

                var docAprovado = _documentoAvalistaRepository.Queryable()
                    .Where(x => x.Avalista.Id == avalista.Id)
                    .Where(x => x.Situacao == SituacaoAprovacaoDocumentoAvalista.PreAprovado)
                    .Any();

                dto.AvalistaDTO.DocsPendentes = !docsPendentes;

                dto.AvalistaDTO.DocsAprovados = docAprovado;
            }

            if (!preProposta.IsEmpty())
            {
                dto.PossuiPosChaves = _planoPagamentoRepository.RegraAvalistaPosChaves(preProposta.Id);
            }
            if(!id.IsEmpty())
                dto.PrePropostaReintegrada = _prePropostaReintegradaRepository.Queryable().Where(x => x.PreProposta.Id == id.Value).Any();

            dto.SimuladorDto = new SimuladorDto();

            //se possui formularios anexados verifica os docs do proponente estão aprovados pro fluxo reduzido
            if (dto.PossuiFormulario && (preProposta.SituacaoProposta == SituacaoProposta.FluxoEnviado || preProposta.SituacaoProposta == SituacaoProposta.AguardandoFluxo))
            {
                var proponentes = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id);

                foreach (var prop in proponentes)
                {
                    if (prop.Titular)
                    {
                        if (_prePropostaService.VerificaDocumentosEmAnaliseCompletaParaAnaliseCompletaAprovada(prop))
                        {
                            dto.hasAprovedDocs = true;
                        }
                    }
                }
            }

            return View(dto);
        }

        private IList<SelectListItem> ProponentesPreProposta(long idPreProposta)
        {
            var request = DataTableExtensions.DefaultRequest();
            var proponentes = _viewProponenteRepository.ProponentesDaProposta(request, idPreProposta);
            IList<SelectListItem> proponentesSelectList = new List<SelectListItem>();
            proponentesSelectList.Add(new SelectListItem() { Value = "0", Text = GlobalMessages.Proponente });
            foreach (var proponente in proponentes.records.ToList())
            {
                proponentesSelectList.Add(new SelectListItem() { Value = proponente.IdCliente.ToString(), Text = proponente.NomeCliente });
            }
            return proponentesSelectList;
        }

        [BaseAuthorize("EVS10", "Visualizar")]
        public JsonResult BuscarDadosCliente(long idCliente)
        {
            var json = new JsonResponse();
            try
            {
                var cliente = _clienteRepository.FindById(idCliente);
                if (!cliente.IsEmpty())
                {
                    json.Objeto = new DadosClienteDTO
                    {
                        Id = cliente.Id,
                        NomeCompleto = cliente.NomeCompleto,
                        CpfCnpj = cliente.CpfCnpj,
                        Email = cliente.Email,
                        TelefoneComercial = cliente.TelefoneComercial,
                        TelefoneResidencial = cliente.TelefoneResidencial
                    };
                }
                json.Sucesso = true;
            }
            catch (Exception ex)
            {
                json.Mensagens.Add(GlobalMessages.MsgErroSelecionarCliente);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderBrevesLancamentos(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {

            var results = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(empresaVenda);

            var list = results.OrderBy(x => x.Nome).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_BreveLancamentoDropdownList", list);
        }

        public ActionResult RenderPropoponenteDropDownList(long idPreProposta)
        {
            var proponentesDaProposta = ProponentesPreProposta(idPreProposta);
            return PartialView("_ProponenteDropDownList", proponentesDaProposta);
        }

        public JsonResult TotalFinanceiro(long idPreProposta)
        {
            CanAccessPreProposta(idPreProposta);

            var proposta = _prePropostaRepository.FindById(idPreProposta);


            var retorno = new
            {
                TotalDetalhamentoFinanceiro = proposta.TotalDetalhamentoFinanceiro,
                Total = proposta.Valor,
                TotalItbiEmolumento = proposta.TotalItbiEmolumento
            };

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult RenderTotalFinanceiro(long idPreProposta)
        {
            CheckAccess(idPreProposta);

            PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);

            _prePropostaService.RecalcularValores(idPreProposta);

            PrePropostaDTO dto = new PrePropostaDTO();
            dto.PreProposta = preProposta;

            return PartialView("_TotalFinanceiro", dto);
        }

        [BaseAuthorize("EVS10", "VisualizarHistoricoPreProposta")]
        public JsonResult BuscarHistoricoPreProposta(DataSourceRequest request, long idPreProposta)
        {
            var result = _viewHistoricoPrePropostaRepository.Listar(request, idPreProposta);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreBaixarBoleto(long idProposta, long idPreProposta)
        {
            JsonResponse res = new JsonResponse();
            SuatService service = new SuatService();

            byte[] file = _prePropostaService.BaixarBoleto(idPreProposta);

            if (file.IsEmpty())
            {
                try
                {
                    MensagemRetornoDownloadDTO boletoPreProposta = service.DownloadBoleto(idProposta, null);
                }
                catch (BusinessRuleException bre)
                {
                    var preProposta = _prePropostaRepository.BuscarPorIdSuat(idProposta);
                    res.Sucesso = false;
                    res.Mensagens.Add(string.Format(GlobalMessages.ErroBaixarBoletoProposta, preProposta.Codigo));
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            res.Sucesso = true;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PreBaixarContrato(long idProposta, long idPreProposta)
        {
            JsonResponse res = new JsonResponse();
            SuatService service = new SuatService();

            byte[] file = _prePropostaService.BaixarContrato(idPreProposta);

            if (file.IsEmpty())
            {
                try
                {
                    MensagemRetornoDownloadDTO contratoPreProposta = service.DownloadContrato(idProposta, null);
                }
                catch (BusinessRuleException bre)
                {
                    var preProposta = _prePropostaRepository.BuscarPorIdSuat(idProposta);
                    res.Sucesso = false;
                    res.Mensagens.Add(string.Format(GlobalMessages.ErroBaixarContratoProposta, preProposta.Codigo));
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
            }
            res.Sucesso = true;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BaixarBoleto(long idProposta, long idPreProposta)
        {
            SuatService service = new SuatService();

            byte[] file = _prePropostaService.BaixarBoleto(idPreProposta);

            if (file.IsNull())
            {
                MensagemRetornoDownloadDTO boletoPreProposta = service.DownloadBoleto(idProposta, null);
                file = boletoPreProposta.Objeto.Bytes;
            }

            var preProposta = _prePropostaRepository.BuscarPorIdSuat(idProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/gif", $"{nomeArquivo}_Boleto_{date}.gif");
        }

        public ActionResult BaixarContrato(long idProposta, long idPreProposta)
        {
            SuatService service = new SuatService();

            byte[] file = _prePropostaService.BaixarContrato(idPreProposta);

            if (file.IsNull())
            {
                MensagemRetornoDownloadDTO contratoPreProposta = service.DownloadContrato(idProposta, null);
                file = contratoPreProposta.Objeto.Bytes;
            }

            var preProposta = _prePropostaRepository.BuscarPorIdSuat(idProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/pdf", $"{nomeArquivo}_Contrato_{date}.pdf");
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult MudarFatorSocial(long idPreProposta, bool? fatorSocial)
        {
            var error = new BusinessRuleException();
            var json = new JsonResponse();
            try
            {
                if (fatorSocial.IsEmpty())
                {
                    error.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.FatorSocial)).Complete();
                }

                error.ThrowIfHasError();

                var result = _prePropostaService.MudarFatorSocial(idPreProposta, fatorSocial.Value);
                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgPrePropostaSucesso, result.Codigo, GlobalMessages.Alterada.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(bre.Errors);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Avalista
        public JsonResult DocumentoAvalista(DataSourceRequest request, long idPreProposta, long? idAvalista,
            SituacaoAprovacaoDocumento? situacaoDocumento)
        {
            CheckAccess(idPreProposta);

            var tiposDocumento = _tipoDocumentoAvalistaRepository.ListarTodos();
            tiposDocumento.ForEach(reg => _session.Evict(reg));

            var avalistas = _prePropostaRepository.ListarDaPreProposta(idPreProposta, idAvalista);
            avalistas.ForEach(reg => _session.Evict(reg));

            var documentosDaProposta =
                _viewDocumentoAvalistaRepository.ListarPreProposta(idPreProposta, idAvalista, null);
            documentosDaProposta.ForEach(reg => _session.Evict(reg));

            foreach (var avalista in avalistas)
            {
                foreach (var tipoDocumento in tiposDocumento)
                {
                    // Verifico se existe documento de determinado tipo para o proponente
                    var documentoAvalista = documentosDaProposta
                        .Where(reg => reg.IdTipoDocumento == tipoDocumento.Id)
                        .Where(reg => reg.IdAvalista == avalista.Id)
                        .SingleOrDefault();

                    if (tipoDocumento.Nome.Contains(" - Não obrigatório"))
                        tipoDocumento.Nome = tipoDocumento.Nome.ToString().Replace(" - Não obrigatório", "");

                    // se não existir, então crio e adiciono na listagem a ser retornada
                    if (documentoAvalista == null)
                    {
                        documentoAvalista = new ViewDocumentoAvalista();
                        if (!avalista.IsEmpty())
                        {
                            documentoAvalista.IdAvalista = avalista.Id;
                            documentoAvalista.NomeAvalista = avalista.Nome;
                        }
                        documentoAvalista.IdTipoDocumento = tipoDocumento.Id;
                        documentoAvalista.IdPreProposta = idPreProposta;
                        documentoAvalista.NomeTipoDocumento = tipoDocumento.Nome;
                        documentoAvalista.Situacao = SituacaoAprovacaoDocumento.NaoAnexado;
                        documentoAvalista.Anexado = false;
                        if (situacaoDocumento.IsEmpty())
                        {
                            documentosDaProposta.Add(documentoAvalista);
                        }
                        else if (situacaoDocumento.Value == documentoAvalista.Situacao)
                        {
                            documentosDaProposta.Add(documentoAvalista);
                        }
                    }
                    else
                    {
                        if (documentoAvalista.NomeTipoDocumento.Contains(" - Não obrigatório"))
                            documentoAvalista.NomeTipoDocumento = documentoAvalista.NomeTipoDocumento.ToString().Replace(" - Não obrigatório", "");
                        documentoAvalista.Anexado = !documentoAvalista.IdArquivo.IsEmpty();
                        if (situacaoDocumento.HasValue() && situacaoDocumento.Value != documentoAvalista.Situacao)
                        {
                            documentosDaProposta.Remove(documentoAvalista);
                        }
                    }
                }
            }

            return Json(documentosDaProposta.AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS10", "IncluirAvalista")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirAvalista(AvalistaDTO dto)
        {
            return SalvarAvalista(dto);
        }

        [HttpPost]
        [BaseAuthorize("EVS10", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarAvalista(AvalistaDTO dto)
        {
            return SalvarAvalista(dto);
        }

        public JsonResult SalvarAvalista(AvalistaDTO dto)
        {
            CanAccessPreProposta(dto.IdPreProposta);

            var isInclusao = dto.Avalista.Id.IsEmpty();

            var result = new JsonResponse();

            try
            {
                _avalistaService.SalvarAvalista(dto.Avalista);
                dto.Endereco.Avalista.Id = dto.Avalista.Id;
                if (dto.Endereco.HasValue())
                {
                    _enderecoAvalistaService.Salvar(dto.Endereco);
                }

                var preProposta = _prePropostaRepository.FindById(dto.IdPreProposta);
                preProposta.Avalista = dto.Avalista;
                _prePropostaService.Salvar(preProposta);

                result.Sucesso = true;
                result.Objeto = dto;
                result.Mensagens.Add(string.Format(GlobalMessages.MsgAvalistaSucesso, dto.Avalista.Nome,
                        isInclusao ? GlobalMessages.Incluido.ToLower() : GlobalMessages.Alterado.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                result.Mensagens.AddRange(bre.Errors);
                result.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadDocumentoAvalista(DocumentoAvalistaDTO documento, HttpPostedFileBase file)
        {
            CheckAccess(documento.IdPreProposta);

            var jsonResponse = new JsonResponse();
            try
            {

                _documentoAvalistaService.SalvarDocumento(documento, file);

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.DocumentoAvalistaCarregadoSucesso,
                    documento.NomeTipoDocumento, documento.NomeAvalista));
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirDocumentoAvalista(DocumentoAvalistaDTO documentoAvalista)
        {

            CheckAccess(documentoAvalista.IdPreProposta);

            var documento = _documentoAvalistaRepository.FindById(documentoAvalista.IdDocumentoAvalista);
            var json = new JsonResponse();
            try
            {
                _documentoAvalistaService.Delete(documento);

                json.Sucesso = true;
                json.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, documento.TipoDocumento.Nome,
                    GlobalMessages.Excluido.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                json.Sucesso = false;
                json.Mensagens.AddRange(bre.Errors);
                _session.Transaction.Rollback();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "BaixarTodosDocumentos")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult BaixarTodosDocumentosAvalista(DocumentoAvalistaDTO documento)
        {
            byte[] file = _documentoAvalistaService.ExportarTodosDocumentos(documento.IdPreProposta, documento.IdAvalista);
            var avalista = _avalistaRepository.FindById(documento.IdAvalista);
            string nomeArquivo = avalista.Nome;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"{nomeArquivo}_Completo_{date}.zip");
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult EnviarDocumentosAvalista(DocumentoAvalistaDTO documento)
        {
            CheckAccess(documento.IdPreProposta);
            var preproposta = _prePropostaRepository.FindById(documento.IdPreProposta);
            var json = new JsonResponse();
            try
            {
                _documentoAvalistaService.EnviarDocumentosAvalista(documento);

                json.Sucesso = true;
                if (preproposta.DocCompleta.HasValue())
                {
                    json.Mensagens.Add(GlobalMessages.DocumentoEnviadoSucesso);
                }
                else
                {
                    json.Mensagens.Add(GlobalMessages.DocumentoAvalistaEnviadoSucesso);
                }
            }
            catch (BusinessRuleException bre)
            {
                json.Sucesso = false;
                json.Mensagens.AddRange(bre.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Detalhamento Financeiro / ITBI / Emolumentos

        [BaseAuthorize("EVS10", "VisualizarDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult DetalhamentoFinanceiro(DataSourceRequest request, long idPreProposta)
        {
            CheckAccess(idPreProposta);
            var results = _prePropostaService.DetalhamentoFinanceiro(request, idPreProposta);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "VisualizarItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ItbiEmolumento(DataSourceRequest request, long idPreProposta)
        {
            CheckAccess(idPreProposta);
            var results = _prePropostaService.ItbiEmolumento(request, idPreProposta);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region simulador
        [HttpPost]
        public JsonResult ResultadoSimulacao(SimuladorDto parametro)
        {
            var response = new JsonResponse();

            try
            {
                var simuladorService = new SimuladorService();
                var simulacao = simuladorService.BuscarSimulacaoPorCodigo(parametro);

                response.Sucesso = simulacao.HasValue();
                response.Objeto = simulacao;
            }
            catch(Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AtualizarResultadosSimulacao(string preProposta)
        {
            var response = new JsonResponse();
            try
            {
                var simuladorService = new SimuladorService();
                var simulacoes = simuladorService.BuscarSimulacaoFinalizadaPorPreProposta(preProposta);

                response.Sucesso = simulacoes.HasValue();                

                if (simulacoes.HasValue())
                {
                    response.Objeto = simulacoes.OrderByDescending(x=>x.Codigo).OrderByDescending(x=>x.Digito).ToList();
                    response.Mensagens.Add("Simulações atualizadas");
                }
            }catch(Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult MontarUrlSimulador(long idPreProposta)
        {
            var response = new JsonResponse();

            try
            {
                var url = _prePropostaService.MontarUrlSimulador(idPreProposta);

                response.Sucesso = true;
                response.Objeto = url;
            }catch(BusinessRuleException bre)
            {
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetalhamentoFinanceiroBySimulador(DataSourceRequest request,SimuladorDto parametro)
        {
            var response = _prePropostaService.DetalhamentoFinanceiroBySimulador(request,parametro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Proponentes

        [BaseAuthorize("EVS10", "VisualizarProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Proponente(DataSourceRequest request, long idPreProposta)
        {
            CheckAccess(idPreProposta);

            var results = _viewProponenteRepository.ProponentesDaProposta(request, idPreProposta);

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Documentação de Proponentes

        /// <summary>
        /// Listar os documentos possíveis e faz um chaveamento entre os documentos preenchidos do proponente
        /// </summary>
        /// <param name="planoPagamento"></param>
        /// <returns></returns>
        public JsonResult DocumentoProponente(DataSourceRequest request, long idPreProposta, long? idCliente, SituacaoAprovacaoDocumento? situacaoDocumento)
        {
            CheckAccess(idPreProposta);

            var proponentes = _viewProponenteRepository.ListarDaPreProposta(idPreProposta, idCliente);
            proponentes.ForEach(reg => _session.Evict(reg));

            var ppr = _prePropostaRepository.FindById(idPreProposta);

            var tiposDocumento = new List<TipoDocumento>();

            if (ppr.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
            {
                tiposDocumento = _tipoDocumentoRepository.ListarTodosPortalEV();
            }
            else
            {
                tiposDocumento = _tipoDocumentoRepository.ListarTodosHouse();
            }
            tiposDocumento.ForEach(reg => _session.Evict(reg));


            var documentosDaProposta = _viewDocumentoProponenteRepository.ListarPreProposta(idPreProposta, idCliente, null);
            documentosDaProposta.ForEach(reg => _session.Evict(reg));

            foreach (var proponente in proponentes)
            {
                foreach (var tipoDocumento in tiposDocumento)
                {

                    // Verifico se existe documento de determinado tipo para o proponente
                    var documentoProponente = documentosDaProposta
                            .Where(reg => reg.IdTipoDocumento == tipoDocumento.Id)
                            .Where(reg => reg.IdCliente == proponente.IdCliente)
                            .SingleOrDefault();

                    // se não existir, então crio e adiciono na listagem a ser retornada
                    if (documentoProponente == null)
                    {
                        documentoProponente = new ViewDocumentoProponente();
                        documentoProponente.IdCliente = proponente.IdCliente;
                        documentoProponente.NomeCliente = proponente.NomeCliente;
                        documentoProponente.IdProponente = proponente.Id;
                        documentoProponente.IdTipoDocumento = tipoDocumento.Id;
                        documentoProponente.IdPreProposta = idPreProposta;
                        documentoProponente.NomeTipoDocumento = tipoDocumento.Nome;
                        documentoProponente.Situacao = SituacaoAprovacaoDocumento.NaoAnexado;
                        documentoProponente.Anexado = false;
                        if (situacaoDocumento.IsEmpty())
                        {
                            documentosDaProposta.Add(documentoProponente);
                        }
                        else if (situacaoDocumento.Value == documentoProponente.Situacao)
                        {
                            documentosDaProposta.Add(documentoProponente);
                        }
                    }
                    else
                    {
                        documentoProponente.Anexado = !documentoProponente.IdArquivo.IsEmpty();
                        if (situacaoDocumento.HasValue() && situacaoDocumento.Value != documentoProponente.Situacao)
                        {
                            documentosDaProposta.Remove(documentoProponente);
                        }
                    }
                }
            }



            return Json(documentosDaProposta.AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "BaixarTodosDocumentos")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult BaixarTodosDocumentos(long idPreProposta, string codigoUf)
        {
            byte[] file = _documentoProponenteService.ExportarTodosDocumentos(idPreProposta, codigoUf);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"{nomeArquivo}_Completo_{date}.zip");
        }

        [BaseAuthorize("EVS10", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadDocumento(ViewDocumentoProponente documento, HttpPostedFileBase file)
        {
            CheckAccess(documento.IdPreProposta);

            var jsonResponse = new JsonResponse();
            try
            {
                var errors = new BusinessRuleException();
                if (file == null || documento == null ||
                    documento.IdPreProposta.IsEmpty() == null ||
                    documento.IdTipoDocumento.IsEmpty() ||
                    documento.IdProponente.IsEmpty())
                {
                    errors.AddError(GlobalMessages.MsgNenhumArquivoSelecionado).Complete();
                }

                errors.ThrowIfHasError();

                var preProposta = _prePropostaRepository.FindById(documento.IdPreProposta);
                var proponente = _proponenteRepository.FindById(documento.IdProponente);
                var tipoDocumento = _tipoDocumentoRepository.FindById(documento.IdTipoDocumento);

                if (preProposta == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado)
                        .WithParams(GlobalMessages.PreProposta, documento.IdPreProposta.ToString()).Complete();
                }

                if (proponente == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado)
                        .WithParams(GlobalMessages.Proponente, documento.IdProponente.ToString()).Complete();
                }

                if (tipoDocumento == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.TipoDocumento,
                        documento.IdTipoDocumento.ToString()).Complete();
                }

                errors.ThrowIfHasError();

                // Se houver documento anexado, ele deve ser utilizado novamente e não criado um novo
                // TODO: Está deixando Arquivos orfãos. Decido manter para histórico.
                DocumentoProponente documentoProponente =
                    _documentoProponenteRepository.BuscarDoTipoParaProponente(documento.IdPreProposta,
                        documento.IdProponente, documento.IdTipoDocumento);
                if (documentoProponente == null)
                {
                    documentoProponente = new DocumentoProponente();
                }

                //if (SituacaoAprovacaoDocumento.Aprovado == documentoProponente.Situacao &&
                //    (!documentoProponente.TipoDocumento.Nome.Equals(GlobalMessages.Documento_ANEXO01) &&
                //    !documentoProponente.TipoDocumento.Nome.Equals(GlobalMessages.Documento_ANEXO07)))
                //{
                //    errors.AddError(GlobalMessages.DocumentoAprovadoNaoPodeModificar)
                //        .WithParams(tipoDocumento.Nome, proponente.Cliente.NomeCompleto).Complete();
                //}

                //errors.ThrowIfHasError();

                var nomeArquivo = string.Format("{0} - {1} - {2}.pdf", preProposta.Codigo,
                    proponente.Cliente.NomeCompleto, tipoDocumento.Nome);
                var arquivo = _arquivoService.CreateFile(file, nomeArquivo);

                _arquivoService.PreencherMetadadosDePdf(ref arquivo);

                documentoProponente.PreProposta = preProposta;
                documentoProponente.Proponente = proponente;

                documentoProponente.Situacao = SituacaoAprovacaoDocumento.Anexado;

                //if (preProposta.SituacaoProposta == SituacaoProposta.FluxoEnviado)
                //{
                //    documentoProponente.Situacao = SituacaoAprovacaoDocumento.Aprovado;
                //}

                documentoProponente.TipoDocumento = tipoDocumento;
                documentoProponente.Motivo = documento.Motivo;
                documentoProponente.Arquivo = arquivo;
                documentoProponente.DataExpiracao = null;

                _documentoProponenteRepository.Save(documentoProponente);

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.DocumentoProponenteCarregadoSucesso,
                    tipoDocumento.Nome, proponente.Cliente.NomeCompleto));
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Integração SUAT
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("EVS05", "IntegrarPrePropostaSUAT")]
        public JsonResult DefinirUnidadePreProposta(long idPreProposta, long idUnidadeSuat, string identificadorUnidadeSuat, long? idTorre, string nomeTorre)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            if (preProposta.IdTorre.IsEmpty())
            {
                preProposta.IdTorre = idTorre.Value;
                preProposta.NomeTorre = nomeTorre;
            }

            // Verificar se a proposta já possui unidade associada
            if (preProposta.IdUnidadeSuat.HasValue() && preProposta.IdUnidadeSuat != idUnidadeSuat)
            {
                var retorno = new MensagemRetornoPropostaDTO();
                retorno.Mensagens.Add(string.Format("A pré-proposta {0} já está associada a unidade de identificador {1}", preProposta.Codigo, preProposta.IdUnidadeSuat));
                retorno.Sucesso = false;
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }

            preProposta.IdUnidadeSuat = idUnidadeSuat;
            preProposta.IdentificadorUnidadeSuat = identificadorUnidadeSuat;
            _prePropostaRepository.Save(preProposta);

            return Json(new { Sucesso = true });
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("EVS05", "IntegrarPrePropostaSUAT")]
        public JsonResult IntegrarPrePropostaSUAT(long idPreProposta)
        {
            var retorno = new MensagemRetornoPropostaDTO();
            try
            {
                var preProposta = _prePropostaRepository.FindById(idPreProposta);
                // Call Validate

                var validationResult = new IntegracaoPrePropostaValidator().Validate(preProposta);
                new BusinessRuleException().WithFluentValidation(validationResult).ThrowIfHasError();

                var dto = MontarPropostaDTO(idPreProposta);
                SuatService service = new SuatService();
                var transaction = CurrentSession().BeginTransaction();

                var result = service.IntegrarPreProposta(dto);
                if (result == null)
                {
                    retorno.Mensagens.Add("A conexão sofreu timeout");
                    retorno.Sucesso = false;
                    return Json(retorno, JsonRequestBehavior.AllowGet);
                }

                retorno.Mensagens.AddRange(result.Mensagens);
                retorno.Sucesso = result.Sucesso;
                retorno.Objeto = result.Objeto;
                // Atualiza os ID SUAT do lado do EVS
                if (retorno.Objeto != null)
                {
                    AtualizarDadosSuat((PropostaDTO)retorno.Objeto, transaction);
                }

                if (!retorno.Sucesso)
                {
                    preProposta.IdUnidadeSuat = 0;
                    preProposta.IdentificadorUnidadeSuat = null;
                    _prePropostaRepository.Save(preProposta);
                }
            }
            catch (BusinessRuleException ex)
            {
                retorno.Mensagens.AddRange(ex.Errors);
                retorno.Campos.AddRange(ex.ErrorsFields);
                retorno.Sucesso = false;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "VisualizarLogIntegracao")]
        public JsonResult ListarLogIntegracao(DataSourceRequest request, string codigoPreProposta)
        {
            SuatService service = new SuatService();
            var logIntegracoes = service.LogIntegracao(codigoPreProposta, 0);
            var results = logIntegracoes.AsQueryable().ToDataRequest(request);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("EVS05", "IntegrarPrePropostaSUAT")]
        public JsonResult DownloadContrato(long idPreProposta)
        {
            var retorno = new MensagemRetornoDownloadDTO();
            try
            {
                SuatService service = new SuatService();
                var preProposta = _prePropostaRepository.FindById(idPreProposta);
                var contrato = _contratoPrePropostaRepository.BuscarContratoMaisRecente(idPreProposta);
                var idContrato = contrato.IsEmpty() ? 0 : contrato.IdContratoSuat;

                var result = service.DownloadContrato(preProposta.IdSuat, idContrato);

                // Retorna sucesso caso seja um novo contrato
                if (result.Sucesso)
                {
                    ContratoPreProposta novoContrato = new ContratoPreProposta();
                    novoContrato.PreProposta = preProposta;
                    novoContrato.Contrato = result.Objeto.Bytes;
                    novoContrato.IdContratoSuat = result.Objeto.Id;
                    _contratoPrePropostaRepository.Save(novoContrato);
                }
                retorno.Mensagens.AddRange(result.Mensagens);
                retorno.Objeto = result.Objeto;
            }
            catch (BusinessRuleException ex)
            {
                retorno.Mensagens.AddRange(ex.Errors);
                retorno.Campos.AddRange(ex.ErrorsFields);
                retorno.Sucesso = false;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("EVS05", "IntegrarPrePropostaSUAT")]
        public JsonResult DownloadBoleto(long idPreProposta)
        {
            var retorno = new MensagemRetornoDownloadDTO();
            try
            {
                SuatService service = new SuatService();
                var preProposta = _prePropostaRepository.FindById(idPreProposta);
                var boleto = _boletoPrePropostaRepository.BuscarBoletoMaisRecente(idPreProposta);
                var idBoleto = boleto.IsEmpty() ? 0 : boleto.Id;

                var result = service.DownloadBoleto(preProposta.IdSuat, idBoleto);

                // Retorna sucesso caso seja um novo contrato
                if (result.Sucesso)
                {
                    BoletoPreProposta novoBoleto = new BoletoPreProposta();
                    novoBoleto.PreProposta = preProposta;
                    novoBoleto.Boleto = result.Objeto.Bytes;
                    novoBoleto.IdBoletoSuat = result.Objeto.Id;
                    _boletoPrePropostaRepository.Save(novoBoleto);
                }
                retorno.Mensagens.AddRange(result.Mensagens);
                retorno.Objeto = result.Objeto;
            }
            catch (BusinessRuleException ex)
            {
                retorno.Mensagens.AddRange(ex.Errors);
                retorno.Campos.AddRange(ex.ErrorsFields);
                retorno.Sucesso = false;
            }

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }



        #endregion


        /// <summary>
        ///  Verifica se o usuário logado pode acessar as informações daquela pré-proposta
        ///  Possivelmente vamos ter que colocar isso como um recurso do framework
        /// </summary>
        /// <param name="idPreProposta"></param>
        /// <returns></returns>
        private bool CanAccessPreProposta(long idPreProposta)
        {
            // Não foi informada pré-proposta
            if (idPreProposta.IsEmpty()) { return true; }

            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            // Não existe a pré-proposta informada
            // FIXME: notFound?
            if (preProposta == null) { return true; }

            // A idéia é tratar com WhiteList (Bloqueio tudo, exceto o que explicitamente estiver liberado
            return true;
        }

        private void CheckAccess(long idPreProposta)
        {
            if (CanAccessPreProposta(idPreProposta)) { return; }

            // FIXME: Tansformar para verbos HTTP
            throw new Exception("Acesso negado");
        }

        private PropostaDTO MontarPropostaDTO(long idPreProposta)
        {
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            var planosPagamento = _planoPagamentoRepository.ListarParcelas(idPreProposta);
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(idPreProposta);
            var endereco = _enderecoClienteRepository.BuscarSomenteEndereco(preProposta.Cliente.Id);
            var enderecoEmpresa = _enderecoEmpresaRepository.BuscarSomenteEndereco(preProposta.Cliente.Id);
            var clienteSuatDTO = new ClienteSuatDTO().FromModel(preProposta.Cliente, endereco, enderecoEmpresa);
            
            PropostaDTO proposta = new PropostaDTO(preProposta, clienteSuatDTO, planosPagamento, proponentes);

            var sicaqEnquadrado = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);
            proposta.SicaqEnquadrado = sicaqEnquadrado;

            foreach (var prop in proposta.Proponentes)
            {
                var cliente = _clienteRepository.FindById(prop.IdCliente);
                if (!cliente.EmpresaVenda.IsEmpty())
                {
                    cliente.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = cliente.EmpresaVenda.Id };
                }
                endereco = _enderecoClienteRepository.BuscarSomenteEndereco(prop.IdCliente);
                enderecoEmpresa = _enderecoEmpresaRepository.BuscarSomenteEndereco(prop.IdCliente);
                proposta.Clientes.Add(new ClienteSuatDTO().FromModel(cliente, endereco, enderecoEmpresa));
            }
            foreach (var clie in proposta.Clientes)
            {
                var clienteConjuge = new Cliente();
                var conjuge = _familiarRepository.BuscarConjugePorCliente(clie.Id);
                if (!conjuge.IsEmpty())
                {
                    clienteConjuge = conjuge.Cliente1.Id == clie.Id ? conjuge.Cliente2 : conjuge.Cliente1;
                    endereco = _enderecoClienteRepository.BuscarSomenteEndereco(clienteConjuge.Id);
                    enderecoEmpresa = _enderecoEmpresaRepository.BuscarSomenteEndereco(clienteConjuge.Id);
                    proposta.Conjuges.Add(new ClienteSuatDTO().FromModel(clienteConjuge, endereco, enderecoEmpresa, clie.Id));
                    clie.IdConjuge = clienteConjuge.Id;
                }
            }

            return proposta;
        }

        private void AtualizarDadosSuat(PropostaDTO dto, ITransaction transaction)
        {
            // Atualizando ID SUAT dos Clientes
            foreach (var clie in dto.Clientes)
            {
                if (!clie.IdSuat.IsEmpty())
                {
                    var cliente = _clienteRepository.FindById(clie.Id);
                    cliente.IdSuat = clie.IdSuat;
                    _clienteRepository.Save(cliente);
                }
            }

            // Atualizando ID SUAT dos Conjuges
            if (dto.Conjuges.HasValue())
            {
                foreach (var clie in dto.Conjuges)
                {
                    if (!clie.IdSuat.IsEmpty())
                    {
                        var cliente = _clienteRepository.FindById(clie.Id);
                        cliente.IdSuat = clie.IdSuat;
                        _clienteRepository.Save(cliente);
                    }
                }
            }

            // Verifica se a Pré-Proposta já tem ID SUAT
            if (!dto.IdSuat.IsEmpty())
            {
                // Atualizando ID SUAT da Pré-Proposta
                var preProposta = _prePropostaRepository.BuscarPorCodigo(dto.ProtocoloGA);
                preProposta.IdSuat = dto.IdSuat;

                //Atualizando situação do passo atual SUAT
                var suatService = new SuatService();
                var resultSituacaoSuat = suatService.BuscarSituacaoProposta(dto.IdSuat);

                if (resultSituacaoSuat.Sucesso)
                {
                    preProposta.PassoAtualSuat = resultSituacaoSuat.Mensagens.First();
                }

                _prePropostaRepository.Save(preProposta);

                // Atualizando ID SUAT dos Proponentes
                foreach (var prop in dto.Proponentes)
                {
                    if (!prop.IdSuat.IsEmpty())
                    {
                        var proponente = _proponenteRepository.FindById(prop.Id);
                        proponente.IdSuat = prop.IdSuat;
                        _proponenteRepository.Save(proponente);
                    }
                }

                // Atualizando ID SUAT dos Planos de Pagamento
                if (dto.PlanosPagamento.HasValue())
                {
                    foreach (var plano in dto.PlanosPagamento)
                    {
                        if (!plano.IdSuat.IsEmpty())
                        {
                            var planoPagamento = _planoPagamentoRepository.FindById(plano.Id);
                            planoPagamento.IdSuat = plano.IdSuat;
                            _planoPagamentoRepository.Save(planoPagamento);
                        }
                    }
                }

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.Integrada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);

            }
        }

        private JsonResponse SalvarPlanoPagamento(PlanoPagamento planoPagamento, long idPreProposta)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var inclusao = planoPagamento.Id == 0;
                _planoPagamentoService.Salvar(planoPagamento, idPreProposta);
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    planoPagamento.TipoParcela.AsString(),
                    inclusao ? GlobalMessages.Incluido.ToLower() : GlobalMessages.Alterado.ToLower()));
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.FromException(bre);
            }

            return jsonResponse;
        }

        private JsonResponse ExcluirPlanoPagamento(long idPreProposta, long idPlanoPagamento)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var planoPagamento = _planoPagamentoService.Excluir(idPlanoPagamento);
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    planoPagamento.TipoParcela.AsString(), GlobalMessages.Excluido.ToLower()));
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                jsonResponse.FromException(ex);
            }

            return jsonResponse;
        }

        [BaseAuthorize("EVS10", "IncluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirDetalhamentoFinanceiro(long idPreProposta, PlanoPagamento planoPagamento)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = SalvarPlanoPagamento(planoPagamento, idPreProposta);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "AlterarDetalhamentoFinanceiro")]

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarDetalhamentoFinanceiro(long idPreProposta, PlanoPagamento planoPagamento)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = SalvarPlanoPagamento(planoPagamento, idPreProposta);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "ExcluirDetalhamentoFinanceiro")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirDetalhamentoFinanceiro(long idPreProposta, long idPlanoPagamento)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = ExcluirPlanoPagamento(idPreProposta, idPlanoPagamento);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ValidarEmpreendimento(long idPreProposta)
        {
            var result = new JsonResponse();
            BusinessRuleException bre = new BusinessRuleException();
            try
            {
                _prePropostaService.ValidarEmpreendimento(idPreProposta);

                result.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                result.Sucesso = false;
                result.Mensagens.AddRange(ex.Errors);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderBrevesLancamentosReenviar(long idEmpresavenda)
        {
            var empresaVenda = _empresaVendaRepository.FindById(idEmpresavenda);
            var results = _enderecoBreveLancamentoRepository.Queryable()
                    .Where(x => x.BreveLancamento.DisponivelCatalogo);
            var list = results.Select(x => x.BreveLancamento);
            if (!empresaVenda.IsEmpty())
            {
                var regionais = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(idEmpresavenda).Select(s=>s.Regional);
                list = results.Where(x => regionais.Contains(x.BreveLancamento.Regional))
                    .Select(x => x.BreveLancamento);
            }
            var result = list.OrderBy(x => x.Nome).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_BreveLancamentoReenviarDropdownList", result);
        }

        [HttpPost]
        [BaseAuthorize("EVS10")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarBreveLancamento(Domain.Dto.PrePropostaDTO prePropostaDTO)
        {
            var response = new JsonResponse();

            try
            {
                _prePropostaService.SalvarBreveLancamento(prePropostaDTO);

                response.Mensagens.Add(string.Format(GlobalMessages.SalvoSucesso));
                response.Sucesso = true;
            }
            catch(BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                response.Mensagens.AddRange(bre.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS10", "ReintegrarPrePropostaSUAT")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarNovoBreveLancamento(long idPreProposta, long? idBreveLancamento)
        {
            var jsonResponse = new JsonResponse();
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            try
            {
                //Salvando dados da preprosta q serão apagados
                _prePropostaReintegradaService.SalvarDadosReintegracao(preProposta);

                //Salva novo breve lançamento e limpa os campos preenchidos pela integraçãoklk
                _prePropostaService.SalvarNovoBreveLancamento(preProposta, idBreveLancamento);

                if (preProposta.SituacaoProposta == SituacaoProposta.Integrada)
                {
                    _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoIntegracao, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                }

                var model = new
                {
                    Divisao = preProposta.BreveLancamento.Empreendimento.HasValue() ? preProposta.BreveLancamento.Empreendimento.Divisao : null,
                    NomeEmpreendimento = preProposta.BreveLancamento.Empreendimento.HasValue() ? preProposta.BreveLancamento.Empreendimento.Nome : preProposta.BreveLancamento.Nome,
                    NomeBreveLancamento = preProposta.BreveLancamento.Nome
                };
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    preProposta.Codigo, GlobalMessages.Alterado.ToLower()));
                jsonResponse.Sucesso = true;
                jsonResponse.Objeto = model;

            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarAutoComplete(DataSourceRequest request)
        {
            var results = _prePropostaRepository.ListarAutoComplete(request, null).Select(reg => new PreProposta
            {
                Id = reg.Id,
                Codigo = reg.Codigo
            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }
    }
}