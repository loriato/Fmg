using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration;
using Tenda.EmpresaVenda.Domain.Integration.Indique.Models;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS10")]
    public class PrePropostaController : BaseController
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private PlanoPagamentoRepository _planoPagamentoRepository { get; set; }
        private ProponenteRepository _proponenteRepository { get; set; }
        private TipoDocumentoRepository _tipoDocumentoRepository { get; set; }
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private BreveLancamentoService _breveLancamentoService { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private ViewPlanoPagamentoRepository _viewPlanoPagamentoRepository { get; set; }
        private ViewProponenteRepository _viewProponenteRepository { get; set; }
        private ViewDocumentoProponenteRepository _viewDocumentoProponenteRepository { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        private MotivoRepository _motivoRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private ProponenteService _proponenteService { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private PlanoPagamentoService _planoPagamentoService { get; set; }
        private DocumentoProponenteService _documentoProponenteService { get; set; }
        private BoletoPrePropostaRepository _boletoPrePropostaRepository { get; set; }
        private ContratoPrePropostaRepository _contratoPrePropostaRepository { get; set; }
        private StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }
        private ViewPrePropostaRepository _viewPrePropostaRepository { get; set; }
        private AvalistaRepository _avalistaRepository { get; set; }
        private ViewDocumentoAvalistaRepository _viewDocumentoAvalistaRepository { get; set; }
        private AvalistaService _avalistaService { get; set; }
        private EnderecoAvalistaRepository _enderecoAvalistaRepository { get; set; }
        private EnderecoAvalistaService _enderecoAvalistaService { get; set; }
        private TipoDocumentoAvalistaRepository _tipoDocumentoAvalistaRepository { get; set; }
        private DocumentoAvalistaService _documentoAvalistaService { get; set; }
        private DocumentoAvalistaRepository _documentoAvalistaRepository { get; set; }
        private PropostaSuatRepository _propostaSuatRepository { get; set; }
        private DocumentoFormularioService _documentoFormularioService { get; set; }
        private DocumentoFormularioRepository _documentoFormularioRepository { get; set; }
        private HistoricoPrePropostaRepository _historicoPrePropostaRepository { get; set; }
        private HierarquiaCoordenadorService _hierarquiaCoordenadorService { get; set; }
        private EstadoCidadeRepository _estadoCidadeRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private HistoricoRecusaIndicacaoService _historicoRecusaIndicacaoService { get; set; }
        private IndiqueService _indiqueService { get; set; }


        #region PreProposta

        [BaseAuthorize("EVS10", "Visualizar")]
        public ActionResult Index(long? id, long? idEmpre, long? idCliente)
        {
            PreProposta preProposta = null;
            PropostaSuat proposta = new PropostaSuat();
            if (id.HasValue)
            {
                try
                {
                    CheckAccess(id.Value);
                }
                catch (Exception bre)
                {
                    ViewBag.Message = bre.Message;
                    ViewBag.StackTrace = bre.StackTrace;
                    return View("Error");
                }
                preProposta = _prePropostaRepository.FindById(id.Value);
                proposta = _propostaSuatRepository.FindByIdPreProposta(preProposta.Id);
            }

            if (idEmpre.HasValue)
            {
                var empre = _breveLancamentoRepository.FindById(idEmpre.Value);
                ViewBag.NomeEmpreendimento = empre.Nome;
                ViewBag.IdEmpreendimento = empre.Id;
            }
            PrePropostaDTO dto = new PrePropostaDTO();
            if (preProposta == null)
            {
                // Elaboração, Status e outros só são definidos ao salvar
                preProposta = new PreProposta();
                preProposta.Corretor = SessionAttributes.Current().Corretor;
                preProposta.Elaborador = SessionAttributes.Current().Corretor;
                preProposta.EmpresaVenda = SessionAttributes.Current().EmpresaVenda;
                dto.ListProponentes = new List<SelectListItem>();
            }
            else
            {
                dto.ListProponentes = ProponentesPreProposta(preProposta.Id);
                dto.SituacaoPrepropostaSuatEvs = _viewPrePropostaRepository.Queryable()
                                                  .Where(x => x.Id == id.Value)
                                                  .Select(reg => reg.SituacaoPrePropostaSuatEvs)
                                                  .SingleOrDefault();

                dto.PossuiFormulario = _documentoFormularioRepository.PrePropostaPossuiFormulario(preProposta.Id);

                var situacaoAnterior = _historicoPrePropostaRepository.Queryable()
                .Where(x => x.PreProposta.Id == preProposta.Id)
                .Where(x => x.Anterior != null);

                if (situacaoAnterior.HasValue())
                {
                    dto.SituacaoAnterior = (int)situacaoAnterior.Select(x => x.Anterior.SituacaoTermino.Value)
                    .FirstOrDefault();
                }
            }

            if (!proposta.IsEmpty())
            {
                dto.KitCompleto = proposta.KitCompleto;
                var pagamento = _planoPagamentoRepository.ListarParcelas(preProposta.Id);
                dto.PermissaoAvalistaPreChaves = _planoPagamentoRepository.RegraAvalistaPreChaves(preProposta.Id);
            }

            if (!proposta.IsEmpty() && !proposta.StatusConformidade.IsEmpty())
            {
                dto.StatusConformidade = proposta.StatusConformidade.ToLower();
            }

            dto.PreProposta = preProposta;
            if (preProposta.BreveLancamento != null)
            {
                var enderecoBreveLancamento =
                    _enderecoBreveLancamentoRepository.FindByBreveLancamento(preProposta.BreveLancamento.Id);
                dto.EnderecoBreveLancamento = enderecoBreveLancamento.ChaveCandidata();
            }

            if (!preProposta.IsEmpty())
            {
                dto.PossuiPosChaves = _planoPagamentoRepository.RegraAvalistaPosChaves(preProposta.Id);
            }

            if (preProposta.Avalista != null)
            {
                dto.AvalistaDTO = new AvalistaDTO();

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
            else
            {
                dto.AvalistaDTO = new AvalistaDTO();
            }

            // Lista os pontos de venda da Empresa de Venda do usuário logado
            dto.ListPontosVenda = _pontoVendaRepository
                .BuscarAtivosPorEmpresaVenda(SessionAttributes.Current().Corretor.EmpresaVenda.Id).Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Nome
                    });

            if (dto.PreProposta.PontoVenda.HasValue() && dto.PreProposta.PontoVenda.Situacao != Situacao.Ativo)
            {
                var pontoVenda = new List<PontoVenda>();
                pontoVenda.Add(dto.PreProposta.PontoVenda);

                dto.ListPontosVenda = pontoVenda.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nome,
                    Selected = true
                });

            }

            // Lista os Breves Lançamentos da Empresa de Venda do usuário logado
            dto.ListBrevesLancamentos = _enderecoBreveLancamentoRepository
                .BuscarPorEstado(SessionAttributes.Current().Corretor.EmpresaVenda.Estado).Select(x =>
                    new SelectListItem
                    {
                        Value = x.BreveLancamento.Id.ToString(),
                        Text = x.BreveLancamento.Nome
                    });
            if (idCliente.HasValue)
            {
                var cliente = _clienteRepository.FindById(idCliente.Value);
                dto.PreProposta.Cliente = cliente;
            }

            dto.ListCidade = _estadoCidadeRepository.Queryable()
                .Where(x => x.Estado.Equals(SessionAttributes.Current().EmpresaVenda.Estado))
                .Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Cidade
                });

            //if (dto.PreProposta.Regiao.HasValue()&&
            //    dto.PreProposta.SituacaoProposta!=SituacaoProposta.EmElaboracao&& 
            //    dto.PreProposta.SituacaoProposta != SituacaoProposta.DocsInsuficientesSimplificado)
            //{
            //    var regiao = new List<EstadoCidade>();
            //    regiao.Add(dto.PreProposta.Regiao);
            //
            //    dto.ListCidade = regiao.Select(x => new SelectListItem
            //    {
            //        Value = x.Id.ToString(),
            //        Text = x.Cidade,
            //        Selected = true
            //    });
            //}

            //if (dto.PreProposta.SituacaoProposta.IsEmpty()||
            //    (dto.PreProposta.SituacaoProposta.HasValue()&& dto.PreProposta.SituacaoProposta.Value==SituacaoProposta.EmElaboracao))
            //{
            //    dto.PreProposta.IsBreveLancamento = true;
            //}
            DocumentosProponentes(dto);

            return View(dto);
        }

        private void DocumentosProponentes(PrePropostaDTO dto)
        {
            var proponentes = _proponenteRepository.ProponentesDaPreProposta(dto.PreProposta.Id);
            var listaDocumentos = new List<DocumentoProponente>();
            var listaArquivos = new Dictionary<string, ArquivoUrlDTO>();
            string webRoot = GetWebAppRoot();

            foreach (var proponente in proponentes)
            {
                var documentos =
                    _documentoProponenteRepository.BuscarDocumentosPorIdPrePropostaIdProponente(dto.PreProposta.Id, proponente.Id)
                    .Where(x => x.Situacao != SituacaoAprovacaoDocumento.Pendente).Where(x => x.Situacao != SituacaoAprovacaoDocumento.Aprovado);
                listaDocumentos.AddRange(documentos.OrderBy(x => x.TipoDocumento.Nome).ToList());
                foreach (var documento in documentos)
                {
                    if (documento.Arquivo.HasValue())
                    {
                        string metadados = "";
                        if (documento.Arquivo.Metadados.IsEmpty() == false)
                        {
                            var objMetadados = JsonConvert.DeserializeObject<Metadado>(documento.Arquivo.Metadados);
                            var objMetadadosFiltrado = new
                            {
                                Titulo = objMetadados.Titulo,
                                Autor = objMetadados.Autor,
                                Assunto = objMetadados.Assunto,
                                CriadoPor = objMetadados.CriadoPor,
                                CriadoEm = objMetadados.CriadoEm,
                                ModificadoEm = objMetadados.ModificadoEm,
                                ProduzidoPor = objMetadados.ProduzidoPor,
                                PalavrasChave = objMetadados.PalavrasChave
                            };

                            metadados = JsonConvert.SerializeObject(objMetadadosFiltrado, Formatting.Indented);
                        }

                        string filename = _staticResourceService.LoadResource(documento.Arquivo.Id);
                        listaArquivos.Add(documento.Id.ToString(), new ArquivoUrlDTO
                        {
                            Url = _staticResourceService.CreateUrl(webRoot, filename),
                            UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename),
                            FileExtension = documento.Arquivo.FileExtension,
                            ContentType = documento.Arquivo.ContentType,
                            Metadados = metadados
                        });
                    }
                }
            }

            dto.Arquivos = listaArquivos;
            dto.Documentos = listaDocumentos.Select(x => new DocumentoProponente
            {
                Id = x.Id,
                Proponente = new Proponente
                {
                    Id = x.Proponente.Id,
                    Cliente = new Cliente
                    {
                        Id = x.Proponente.Cliente.Id,
                        NomeCompleto = x.Proponente.Cliente.NomeCompleto
                    }
                },
                TipoDocumento = x.TipoDocumento,
                Motivo = x.Motivo,
                Situacao = x.Situacao,
                DataExpiracao = x.DataExpiracao,
            }).ToList();
        }

        private IList<SelectListItem> ProponentesPreProposta(long idPreProposta)
        {
            var request = DataTableExtensions.DefaultRequest();
            var proponentes = _viewProponenteRepository.ProponentesDaProposta(request, idPreProposta);
            IList<SelectListItem> proponentesSelectList = new List<SelectListItem>();
            proponentesSelectList.Add(new SelectListItem() { Value = "0", Text = GlobalMessages.Proponente });
            foreach (var proponente in proponentes.records.ToList())
            {
                proponentesSelectList.Add(new SelectListItem()
                { Value = proponente.IdCliente.ToString(), Text = proponente.NomeCliente });
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

        [BaseAuthorize("EVS10", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(PrePropostaDTO dto)
        {
            return Salvar(dto);
        }

        [BaseAuthorize("EVS10", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Alterar(PrePropostaDTO dto)
        {
            return Salvar(dto);
        }


        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Enviar(PrePropostaDTO dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            JsonResponse response = new JsonResponse();

            BusinessRuleException error = new BusinessRuleException();
            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.PreProposta.Id);
                _prePropostaService.ValidarRateioComissao(preProposta);
                //Salvando valor parcela solicitada se o usuario clickou em enviar direto
                preProposta.ParcelaSolicitada = dto.PreProposta.ParcelaSolicitada;

                var brevaLancamaento = _breveLancamentoRepository.FindById(dto.IdBreveLancamento);

                preProposta.BreveLancamento = brevaLancamaento;
                preProposta.IdTorre = dto.PreProposta.IdTorre;
                preProposta.NomeTorre = dto.PreProposta.NomeTorre;
                preProposta.ObservacaoTorre = dto.PreProposta.ObservacaoTorre;
                preProposta.FaixaEv = dto.PreProposta.FaixaEv;
                //preProposta.IsBreveLancamento = dto.PreProposta.IsBreveLancamento;

                if (preProposta.SituacaoProposta != SituacaoProposta.AguardandoFluxo)
                {
                    //var regiao = _estadoCidadeRepository.FindById(dto.PreProposta.Regiao.Id);
                    //preProposta.Regiao = regiao;
                }

                SituacaoProposta situacaoDestino = preProposta.SituacaoProposta.Value;

                switch (preProposta.SituacaoProposta)
                {
                    case SituacaoProposta.EmElaboracao:
                        situacaoDestino = SituacaoProposta.AguardandoAnaliseSimplificada;
                        break;

                    case SituacaoProposta.AguardandoFluxo:
                        situacaoDestino = SituacaoProposta.FluxoEnviado;
                        break;

                    default:
                        error.AddError(string.Format(GlobalMessages.PreProposta_Inconsistente, preProposta.Codigo));
                        error.ThrowIfHasError();
                        break;
                }

                _prePropostaService.MudarSituacaoProposta(preProposta, situacaoDestino,
                    SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);

                if (preProposta.SituacaoProposta == SituacaoProposta.AguardandoAnaliseSimplificada)
                {
                    _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                }

                response.Sucesso = true;
                response.Mensagens.Add(string.Format(GlobalMessages.PreProposta_EnviadaSucesso,
                    preProposta.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Retornar(PrePropostaDTO dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            JsonResponse response = new JsonResponse();
            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.PreProposta.Id);

                var brevaLancamaento = _breveLancamentoRepository.FindById(dto.IdBreveLancamento);

                preProposta.BreveLancamento = brevaLancamaento;
                preProposta.IdTorre = dto.PreProposta.IdTorre;
                preProposta.NomeTorre = dto.PreProposta.NomeTorre;
                preProposta.ObservacaoTorre = dto.PreProposta.ObservacaoTorre;
                preProposta.FaixaEv = dto.PreProposta.FaixaEv;

                //if (dto.PreProposta.Regiao.IsEmpty())
                //{
                //    preProposta.Regiao = null;
                //}
                //else
                //{
                //    var regiao = _estadoCidadeRepository.FindById(dto.PreProposta.Regiao.Id);
                //
                //    preProposta.Regiao = regiao;
                //}

                //Salvando valor parcela solicitada se o usuario clickou em enviar direto
                preProposta.ParcelaSolicitada = dto.PreProposta.ParcelaSolicitada;
                var situacao = _prePropostaService.SituacaoAnterior(dto.PreProposta.Id);

                switch (situacao)
                {
                    case SituacaoProposta.EmAnaliseCompleta:
                        situacao = SituacaoProposta.AguardandoAnaliseCompleta;
                        break;
                    case SituacaoProposta.EmAnaliseSimplificada:
                        situacao = SituacaoProposta.AguardandoAnaliseSimplificada;
                        break;
                    case SituacaoProposta.AnaliseSimplificadaAprovada:
                        situacao = SituacaoProposta.AguardandoAnaliseSimplificada;
                        break;
                    default:
                        situacao = SituacaoProposta.EmElaboracao;
                        break;
                }

                _prePropostaService.MudarSituacaoProposta(preProposta, situacao.Value,
                    SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("A proposta de código {0} foi enviada com sucesso!",
                    preProposta.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AguardandoAnaliseCompleta(PrePropostaDTO dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            JsonResponse response = new JsonResponse();
            try
            {
                var preProposta = _prePropostaRepository.FindById(dto.PreProposta.Id);

                var breveLancamento = _breveLancamentoRepository.FindById(dto.PreProposta.BreveLancamento.Id);

                preProposta.BreveLancamento = breveLancamento;
                preProposta.IdTorre = dto.PreProposta.IdTorre;
                preProposta.NomeTorre = dto.PreProposta.NomeTorre;
                preProposta.ObservacaoTorre = dto.PreProposta.ObservacaoTorre;

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseCompleta,
                    SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("A proposta de código {0} foi enviada com sucesso!",
                    preProposta.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(response);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarCorretor(long idPreProposta, long? idCorretor)
        {
            CanAccessPreProposta(idPreProposta);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            JsonResponse response = new JsonResponse();
            try
            {
                _prePropostaService.AlterarCorretor(idPreProposta, idCorretor);
                var corretor = _corretorRepository.FindById(idCorretor.Value);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format("O corretor {0} foi definido como corretor da proposta {1}",
                corretor.Nome, preProposta.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Cancelar(long idPreProposta)
        {
            CanAccessPreProposta(idPreProposta);

            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.Cancelada,
                SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);

            JsonResponse response = new JsonResponse();
            response.Sucesso = true;
            response.Mensagens.Add(string.Format("A proposta de código {0} foi cancelada com sucesso!",
                preProposta.Codigo));
            return Json(response);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Revisar(long idPreProposta)
        {
            CanAccessPreProposta(idPreProposta);

            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            _prePropostaService.RevisarPreProposta(preProposta);

            JsonResponse response = new JsonResponse();
            response.Sucesso = true;
            response.Mensagens.Add(string.Format("A proposta de código {0} foi revisada com sucesso!",
                preProposta.Codigo));
            return Json(response);
        }

        private JsonResult Salvar(PrePropostaDTO dto)
        {
            CanAccessPreProposta(dto.PreProposta.Id);

            var isInclusao = dto.PreProposta.Id.IsEmpty();
            var aux = dto.PreProposta;

            // Quando edição, só posso mexer no breve lançamento
            if (!isInclusao)
            {
                var proposta = _prePropostaRepository.FindById(dto.PreProposta.Id);

                if (dto.PreProposta.BreveLancamento.HasValue() && dto.PreProposta.BreveLancamento.Id.HasValue())
                {
                    proposta.BreveLancamento = _breveLancamentoRepository.FindById(dto.PreProposta.BreveLancamento.Id);
                }
                else
                {
                    proposta.BreveLancamento = null;
                }

                //var regiao = _estadoCidadeRepository.FindById(dto.PreProposta.Regiao.Id);
                //proposta.Regiao = regiao;

                proposta.ParcelaSolicitada = dto.PreProposta.ParcelaSolicitada;
                dto.PreProposta = proposta;
            }
            else if (!dto.PreProposta.PontoVenda.Id.IsEmpty())
            {
                var pontovenda = _pontoVendaRepository.FindById(dto.PreProposta.PontoVenda.Id);
                dto.PreProposta.Viabilizador = pontovenda.Viabilizador;
            }

            if (aux.IdTorre != dto.PreProposta.IdTorre)
            {
                dto.PreProposta.IdTorre = aux.IdTorre;
                dto.PreProposta.NomeTorre = aux.NomeTorre;
            }

            var json = new JsonResponse();
            try
            {
                //dto.PreProposta.IsBreveLancamento = aux.IsBreveLancamento;
                dto.PreProposta.FaixaEv = aux.FaixaEv;
                dto.PreProposta.ObservacaoTorre = aux.ObservacaoTorre;

                if (!dto.PreProposta.BreveLancamento.IsEmpty())
                {
                    dto.PreProposta.BreveLancamento =
                        _breveLancamentoRepository.FindById(dto.PreProposta.BreveLancamento.Id);
                }
                else
                {
                    dto.PreProposta.BreveLancamento = null;
                }

                if (!dto.PreProposta.PontoVenda.IsEmpty())
                {
                    dto.PreProposta.PontoVenda = _pontoVendaRepository.FindById(dto.PreProposta.PontoVenda.Id);
                }
                else
                {
                    dto.PreProposta.PontoVenda = null;
                }

                if (!dto.PreProposta.Cliente.IsEmpty())
                {
                    dto.PreProposta.Cliente = _clienteRepository.FindById(dto.PreProposta.Cliente.Id);
                }
                else
                {
                    dto.PreProposta.Cliente = null;
                }

                _prePropostaService.Salvar(dto.PreProposta);

                // Cria histórico da Pré Proposta
                if (isInclusao)
                {
                    _prePropostaService.MudarSituacaoProposta(dto.PreProposta, SituacaoProposta.EmElaboracao,
                        SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                    var webRoot = GetWebAppRootAdmin();
                    _hierarquiaCoordenadorService.NotificarClienteDuplicado(webRoot, dto.PreProposta);

                }

                // Lista os pontos de venda da Empresa de Venda do usuário logado
                dto.ListPontosVenda = _pontoVendaRepository
                    .BuscarAtivosPorEmpresaVenda(SessionAttributes.Current().Corretor.EmpresaVenda.Id).Select(x =>
                        new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Nome
                        });

                // Lista os Breves Lançamentos da Empresa de Venda do usuário logado
                dto.ListBrevesLancamentos = _enderecoBreveLancamentoRepository
                    .BuscarPorEstado(SessionAttributes.Current().Corretor.EmpresaVenda.Estado).Select(x =>
                        new SelectListItem
                        {
                            Value = x.BreveLancamento.Id.ToString(),
                            Text = x.BreveLancamento.Nome
                        });

                //Lista de cidade
                dto.ListCidade = _estadoCidadeRepository.Queryable()
                .Where(x => x.Estado.Equals(SessionAttributes.Current().EmpresaVenda.Estado))
                .Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Cidade
                });

                var result = new
                {
                    htmlGeral = RenderRazorViewToString("_Geral", dto, false)
                };

                json.Objeto = result;
                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgPrePropostaSucesso, dto.PreProposta.Codigo,
                    isInclusao ? GlobalMessages.Incluida.ToLower() : GlobalMessages.Alterada.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(bre.Errors);
                json.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RenderBrevesLancamentos()
        {
            var request = DataTableExtensions.DefaultRequest();
            request.pageSize = 50;

            var empresaVenda = SessionAttributes.Current().EmpresaVenda;

            var results = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(empresaVenda);

            var list = results.OrderBy(x => x.Nome).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_BreveLancamentoDropdownList", list);
        }
        public ActionResult RenderBrevesLancamentosClone()
        {
            var request = DataTableExtensions.DefaultRequest();
            request.pageSize = 50;

            var empresaVenda = SessionAttributes.Current().EmpresaVenda;

            var results = _breveLancamentoService.DisponiveisParaCatalogoNoEstado(empresaVenda);

            var list = results.OrderBy(x => x.Nome).Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_BreveLancamentoDropdownListClone", list);
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

        [BaseAuthorize("EVS10", "Clonar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ClonarPreProposta(long idPreProposta, long? idBreveLancamento, long? idTorre, string obsTorre, string nomeTorre)
        {
            JsonResponse response = new JsonResponse();
            PreProposta clone = null;
            try
            {
                var origem = _prePropostaRepository.FindById(idPreProposta);
                var codigoOrigem = "";
                if (!origem.IsEmpty())
                {
                    codigoOrigem = origem.Codigo;
                }
                clone = _prePropostaService.ClonarPreProposta(idPreProposta, idBreveLancamento, idTorre, obsTorre, nomeTorre);

                // Forçando que a PreProposta seja criada no banco de dados
                // Ao fazer isso preciso garantir o controle para a remoção dos dados caso haja problema nos próximos passos.
                CurrentTransaction().Commit();
                _transaction = _session.BeginTransaction();

                _prePropostaService.ClonarPreProposta(clone, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);

                response.Sucesso = true;

                CurrentTransaction().Commit();
                _transaction = _session.BeginTransaction();

                var prePropostaClonada = new PreProposta();
                prePropostaClonada.Id = clone.Id;
                response.Objeto = prePropostaClonada;
                response.Mensagens.Add(string.Format(GlobalMessages.MsgClonePrePropostaSucesso, codigoOrigem, clone.Codigo));
            }
            catch (BusinessRuleException bre)
            {
                if (CurrentTransaction() != null && CurrentTransaction().IsActive)
                {
                    CurrentTransaction().Commit();
                }

                // Para remover a preProposta caso o erro tenha sido no avanço
                _transaction = _session.BeginTransaction();
                _prePropostaService.RemoverClonePorFalha(clone);

                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
                response.Campos.AddRange(bre.ErrorsFields);
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);

                if (CurrentTransaction() != null && CurrentTransaction().IsActive)
                {
                    CurrentTransaction().Commit();
                }

                // Para remover a preProposta caso o erro tenha sido no avanço
                _transaction = _session.BeginTransaction();
                _prePropostaService.RemoverClonePorFalha(clone);

                response.Sucesso = false;
                response.Mensagens.Add(e.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
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

        [BaseAuthorize("EVS10", "VisualizarItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ItbiEmolumento(DataSourceRequest request, long idPreProposta)
        {
            CheckAccess(idPreProposta);
            var results = _prePropostaService.ItbiEmolumento(request, idPreProposta);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "IncluirItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirItbiEmolumento(long idPreProposta, PlanoPagamento planoPagamento)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = SalvarPlanoPagamento(planoPagamento, idPreProposta);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "AlterarItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarItbiEmolumento(long idPreProposta, PlanoPagamento planoPagamento)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = SalvarPlanoPagamento(planoPagamento, idPreProposta);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "ExcluirItbiEmolumento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirItbiEmolumento(long idPreProposta, long idPlanoPagamento)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = ExcluirPlanoPagamento(idPreProposta, idPlanoPagamento);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
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

        [BaseAuthorize("EVS10", "IncluirProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirProponente(long idPreProposta, Proponente proponente, long idCliente)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = SalvarProponente(proponente, idPreProposta, idCliente);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "AlterarProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarProponente(long idPreProposta, Proponente proponente, long idCliente)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = SalvarProponente(proponente, idPreProposta, idCliente);
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS10", "ExcluirProponente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirProponente(long idPreProposta, long idProponente)
        {
            CheckAccess(idPreProposta);
            var jsonResponse = new JsonResponse();
            try
            {

                var proponente = _proponenteService.Excluir(idProponente);
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    proponente.Cliente.NomeCompleto, GlobalMessages.Excluido.ToLower()));
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                jsonResponse.FromException(ex);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        private JsonResponse SalvarProponente(Proponente proponente, long idPreProposta, long idCliente)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var inclusao = proponente.Id == 0;
                _proponenteService.Salvar(proponente, idPreProposta, idCliente);
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    proponente.Cliente.NomeCompleto,
                    inclusao ? GlobalMessages.Incluido.ToLower() : GlobalMessages.Alterado.ToLower()));
                jsonResponse.Objeto = new { idProponente = proponente.Id };
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.FromException(bre);
            }

            return jsonResponse;
        }

        #endregion

        #region Documentação de Proponentes

        /// <summary>
        /// Listar os documentos possíveis e faz um chaveamento entre os documentos preenchidos do proponente
        /// </summary>
        /// <param name="planoPagamento"></param>
        /// <returns></returns>
        public JsonResult DocumentoProponente(DataSourceRequest request, long idPreProposta, long? idCliente,
            SituacaoAprovacaoDocumento? situacaoDocumento)
        {
            CheckAccess(idPreProposta);

            var tiposDocumento = _tipoDocumentoRepository.ListarTodosPortalEV();
            tiposDocumento.ForEach(reg => _session.Evict(reg));

            var proponentes = _viewProponenteRepository.ListarDaPreProposta(idPreProposta, idCliente);
            proponentes.ForEach(reg => _session.Evict(reg));

            var documentosDaProposta =
                _viewDocumentoProponenteRepository.ListarPreProposta(idPreProposta, idCliente, null);
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

                    if (tipoDocumento.Nome.Contains(" - Não obrigatório"))
                        tipoDocumento.Nome = tipoDocumento.Nome.ToString().Replace(" - Não obrigatório", "");

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
                        if (documentoProponente.NomeTipoDocumento.Contains(" - Não obrigatório"))
                            documentoProponente.NomeTipoDocumento = documentoProponente.NomeTipoDocumento.ToString().Replace(" - Não obrigatório", "");
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

        [BaseAuthorize("EVS10", "BaixarTodosDocumentos")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult BaixarDocumento(long idPreProposta, long idDocumento)
        {
            byte[] file = _documentoProponenteService.ExportarDocumento(idDocumento);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"{nomeArquivo}_{date}.zip");
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ExcluirDocumentoProponente(ViewDocumentoProponente documentoProponente)
        {

            CheckAccess(documentoProponente.IdPreProposta);

            var documento = _documentoProponenteRepository.FindById(documentoProponente.Id);
            var json = new JsonResponse();
            try
            {
                _documentoProponenteRepository.Delete(documento);
                _arquivoRepository.Delete(documento.Arquivo);
                CurrentTransaction().Commit();
                json.Sucesso = true;
                json.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, documento.TipoDocumento.Nome,
                    GlobalMessages.Excluido.ToLower()));
            }
            catch (GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    json.Mensagens.Add(String.Format(GlobalMessages.ErroExcluirDocumentoPreProposta));
                }
                else
                {
                    json.Mensagens.Add(String.Format(GlobalMessages.ErroNaoTratado, ex.Message));
                }
                _arquivoRepository._session.Transaction.Rollback();
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


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
                    //errors.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.PreProposta, documento.IdPreProposta.ToString()).Complete();
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

                if (SituacaoAprovacaoDocumento.Aprovado == documentoProponente.Situacao)
                {
                    errors.AddError(GlobalMessages.DocumentoAprovadoNaoPodeModificar)
                        .WithParams(tipoDocumento.Nome, proponente.Cliente.NomeCompleto).Complete();
                }

                errors.ThrowIfHasError();

                var nomeArquivo = string.Format("{0} - {1} - {2}.pdf", preProposta.Codigo,
                    proponente.Cliente.NomeCompleto, tipoDocumento.Nome);
                var arquivo = _arquivoService.CreateFile(file, nomeArquivo);

                _arquivoService.PreencherMetadadosDePdf(ref arquivo);

                documentoProponente.PreProposta = preProposta;
                documentoProponente.Proponente = proponente;
                documentoProponente.Situacao = SituacaoAprovacaoDocumento.Anexado;
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

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult NegativaAnexarDocumentoProponente(ViewDocumentoProponente documento, long? idMotivo,
            string descricaoMotivo)
        {
            var json = new JsonResponse();
            try
            {
                CheckAccess(documento.IdPreProposta);
                var errors = new BusinessRuleException();

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

                DocumentoProponente documentoProponente =
                    _documentoProponenteRepository.BuscarDoTipoParaProponente(documento.IdPreProposta,
                        documento.IdProponente, documento.IdTipoDocumento);
                if (documentoProponente == null)
                {
                    documentoProponente = new DocumentoProponente();
                }

                // Não permitir alterar caso o documento já tenha sido informado
                if (SituacaoAprovacaoDocumento.Aprovado == documentoProponente.Situacao)
                {
                    errors.AddError(GlobalMessages.DocumentoAprovadoNaoPodeModificar)
                        .WithParams(tipoDocumento.Nome, proponente.Cliente.NomeCompleto).Complete();
                }

                errors.ThrowIfHasError();

                // Verificando motivo
                ValidarMotivo(idMotivo, descricaoMotivo);
                if (idMotivo > 0)
                {
                    var motivo = _motivoRepository.FindById(idMotivo.Value);

                    // Verificando se o o motivo de anexado em outro proponente procede
                    if (motivo.Id == ProjectProperties.MotivoDocumentoAnexadoOutroProponente)
                    {
                        var possuiAnexado =
                            _documentoProponenteRepository.PossuiDocumentoAnexadoEmOutroProponente(preProposta.Id,
                                proponente.Id, tipoDocumento.Id);

                        if (!possuiAnexado)
                        {
                            errors.AddError(GlobalMessages.NaoFoiEncontradoDocumentoAnexado)
                                .WithParam(tipoDocumento.Nome).Complete();
                            errors.ThrowIfHasError();
                        }
                    }

                    documentoProponente.Motivo = motivo.Descricao;
                }
                else
                {
                    documentoProponente.Motivo = descricaoMotivo;
                }

                documentoProponente.PreProposta = preProposta;
                documentoProponente.Proponente = proponente;
                documentoProponente.Situacao = SituacaoAprovacaoDocumento.Informado;
                documentoProponente.TipoDocumento = tipoDocumento;
                documentoProponente.DataExpiracao = null;

                _documentoProponenteRepository.Save(documentoProponente);
                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, documentoProponente.TipoDocumento.Nome,
                    GlobalMessages.Atualizado.ToLower()));
            }
            catch (BusinessRuleException bre)
            {
                json.Mensagens.AddRange(bre.Errors);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private void ValidarMotivo(long? idMotivo, string descricaoMotivo)
        {
            var bre = new BusinessRuleException();
            if (idMotivo.IsNull())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Motivo)).Complete();
            }

            bre.ThrowIfHasError();
            if (idMotivo == 0 && descricaoMotivo.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Detalhe)).Complete();
            }

            bre.ThrowIfHasError();
            if (idMotivo.IsEmpty() && descricaoMotivo.Length < 2)
            {
                bre.AddError(string.Format(GlobalMessages.CampoTamanhoMinimo, GlobalMessages.Detalhe, 1)).Complete();
            }

            bre.ThrowIfHasError();
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
            if (idPreProposta.IsEmpty())
            {
                return true;
            }

            var preProposta = _prePropostaRepository.FindById(idPreProposta);

            // Não existe a pré-proposta informada
            // FIXME: notFound?
            if (preProposta == null)
            {
                return true;
            }

            // A pré-proposta é da mesma empresa de venda, pode exibir
            if (preProposta.EmpresaVenda.Id == base.EmpresaVendaId)
            {
                return true;
            }

            // A idéia é tratar com WhiteList (Bloqueio tudo, exceto o que explicitamente estiver liberado
            return false;
        }

        private void CheckAccess(long idPreProposta)
        {

            if (CanAccessPreProposta(idPreProposta))
            {
                return;
            }

            // FIXME: Tansformar para verbos HTTP
            throw new Exception("Acesso negado");
        }

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
        [BaseAuthorize("EVS10", "Incluir")]
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

        public ActionResult ListarAutoComplete(DataSourceRequest request)
        {
            var results = _prePropostaRepository.ListarAutoComplete(request, SessionAttributes.Current().EmpresaVenda.Id).Select(reg => new PreProposta
            {
                Id = reg.Id,
                Codigo = reg.Codigo
            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public FileContentResult BaixarFormularios(long idPreProposta)
        {
            byte[] file = _documentoFormularioService.BaixarFormularios(idPreProposta);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            string nomeArquivo = preProposta.Codigo;
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"Formularios_{nomeArquivo}_Completo_{date}.zip");
        }

        public ActionResult ListarEstadoIndiqueAutoComplete(DataSourceRequest request)
        {
            var indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var result = indiqueService.ListarEstadoAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListarCidadeIndiqueAutoComplete(DataSourceRequest request)
        {
            var indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();

            var result = indiqueService.ListarCidadeAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarIndicacao(List<IndicadoDto> indicacaoDto, long idCliente)
        {
            var baseResponse = new List<BaseResponse>();

            var indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var cliente = _clienteRepository.FindById(idCliente);
            var clienteMgm = indiqueService.BuscarClienteLandingPage(cliente.CpfCnpj);

            foreach (var indicacao in indicacaoDto)
            {

                indicacao.IdCliente = clienteMgm.Id;
                var result = indiqueService.CadastrarIndicado(indicacao);
                result.Data = indicacao;
                baseResponse.Add(result);


            }
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarRecusaIndicacao(long idPreProposta)
        {
            var baseResponse = new BaseResponse();
            var usuario = SessionAttributes.Current().UsuarioPortal;
            var preproposta = _prePropostaRepository.FindById(idPreProposta);
            var model = new HistoricoRecusaIndicacao
            {
                PreProposta = preproposta,
                Responsavel = usuario,
                DataMomento = DateTime.Now,
                SituacaoPreProposta = preproposta.SituacaoProposta.Value
            };
            var recusaIndicacao = _historicoRecusaIndicacaoService.Salvar(model);
            baseResponse.Success = true;
            baseResponse.Data = recusaIndicacao.Id;

            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarWhatsBotmakerIndicado(long id, string nome, string telefone)
        {
            var baseResponse = _indiqueService.EnviarWhatsBotmakerIndicado(id, nome, telefone);
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarWhatsBotmakerIndicador(long id)
        {
            var baseResponse = _indiqueService.EnviarWhatsBotmakerIndicador(id);
            return Json(baseResponse, JsonRequestBehavior.AllowGet);
        }
    }

}