using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json.Linq;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.RegraComissao;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS01")]
    public class EmpresaVendaController : BaseController
    {
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ViewEmpresaVendaRepository _viewEmpresaVendaRepository { get; set; }
        private EmpresaVendaService _empresaVendaService { get; set; }
        private CorretorService _corretorService { get; set; }
        private PontoVendaService _pontoVendaService { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }
        private DocumentoEmpresaVendaService _documentoEmpresaVendaService { get; set; }
        private DocumentoEmpresaVendaRepository _documentoEmpresaVendaRepository { get; set; }
        private RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        private RegionalEmpresaService _regionalEmpresaService { get; set; }
        private RegionaisRepository _regionaisRepository { get; set; }
        public ViewResponsavelAceiteRegraComissaoRepository _viewResponsavelAceiteRegraComissaoRepository { get; set; }

        [BaseAuthorize("EVS01", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS01", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, string empresaVenda, string cnpj, string creci,
            List<Situacao> situacoes, List<long> regionais,List<string> estados)
        {
            var results = _viewEmpresaVendaRepository.Listar(empresaVenda, cnpj, creci, situacoes, regionais, estados);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS01", "Visualizar")]
        public ActionResult ListarAutoComplete(DataSourceRequest request)
        {
            var results = _empresaVendaService.Listar(request).Select(reg =>
                new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda
                {
                    Id = reg.Id,
                    NomeFantasia = reg.NomeFantasia
                });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarTree(long? idRegraReferencia, String regional)
        {
            if (idRegraReferencia.IsEmpty() && regional.IsEmpty())
            {
                return Json(new List<TreeViewModel>(), JsonRequestBehavior.AllowGet);
            }

            var empresas = _empresaVendaRepository.EmpresasDaRegional(regional).Where(x => x.Situacao == Situacao.Ativo);

            var empresasComRegras = new List<long>();

            if (idRegraReferencia.HasValue())
            {
                empresasComRegras = _regraComissaoEvsRepository.BuscarPorRegraComissao(idRegraReferencia.Value)
                    .Select(x => x.EmpresaVenda.Id)
                    .ToList();
            }

            var results = empresas.Select(x => new TreeViewModel
            {
                id = x.Id,
                text = x.NomeFantasia,
                selectable = true,
                state = new TreeViewState
                {
                    @checked = empresasComRegras.Contains(x.Id),
                    disabled = false,
                    expanded = false,
                    selected = false
                }
            }).OrderBy(x => x.text);
            return Json(results.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListarTreeIdEvs(long[] idEvs)
        {
            var empresas = _empresaVendaRepository.FindByIdsEmpresaVenda(idEvs.ToList());

            var results = empresas.Select(x => new TreeViewModel
            {
                id = x.Id,
                text = x.NomeFantasia,
                selectable = true,
                state = new TreeViewState
                {
                    @checked = false,
                    disabled = false,
                    expanded = false,
                    selected = false
                }
            }).OrderBy(x => x.text);
            return Json(results.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarTreeEv(string estado, string empresaVenda, bool isCargaLead = false)
        {
            if (estado.IsEmpty() && empresaVenda.IsEmpty() && isCargaLead == false)
            {
                return Json(new List<TreeViewModel>(), JsonRequestBehavior.AllowGet);
            }

            var empresas = _empresaVendaRepository.EmpresasTreeEv(estado, empresaVenda);


            var results = empresas.Select(x => new TreeViewModel
            {
                id = x.Id,
                text = x.NomeFantasia,
                selectable = true,
                state = new TreeViewState
                {
                    @checked = false,
                    disabled = false,
                    expanded = false,
                    selected = false
                }
            }).OrderBy(x => x.text);
            return Json(results.ToList(), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS01", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(EmpresaVendaDTO empresaVendaDTO)
        {
            if (Request.Files.HasValue())
            {
                var files = Request.Files;

                for (var y = 0; y < files.Count; y++)
                {
                    for (var i = 0; i < empresaVendaDTO.Documentos.Count; i++)
                    {
                        if (empresaVendaDTO.Documentos[i].NomeArquivo.Equals(files[y].FileName))
                        {
                            empresaVendaDTO.Documentos[i].File = files[y];
                        }
                    }
                }
            }

            return Salvar(empresaVendaDTO.EmpresaVenda, empresaVendaDTO.Corretor,
                empresaVendaDTO.EnderecoCorretor, empresaVendaDTO.Documentos,
                empresaVendaDTO.idsRegionais);
        }

        [BaseAuthorize("EVS01", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Atualizar(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Corretor corretor,
            EnderecoCorretor enderecoCorretor, List<DocumentoEmpresaVendaDto> documentos,List<long> idsRegionais)
        {
            return Salvar(empresaVenda, corretor, enderecoCorretor, documentos,
                idsRegionais);
        }

        private JsonResult Salvar(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Corretor corretor,
            EnderecoCorretor enderecoCorretor, List<DocumentoEmpresaVendaDto> documentos,
            List<long> idsRegionais)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                empresaVenda.CentralVendas = "Pendente Remoção";
                var novaEmpresaVenda = empresaVenda.Id == 0;

                if (novaEmpresaVenda)
                {
                    if (ProjectProperties.IdPerfilDiretorPortalEvs.IsEmpty())
                    {
                        validation.AddError(GlobalMessages.ParametroInexistente).WithParams(GlobalMessages.PerfilDiretorPortalEvs).Complete();
                    }
                    validation.ThrowIfHasError();
                }
                if(idsRegionais==null||idsRegionais.Count==0)
                {
                    validation.AddError(GlobalMessages.SelecionarRegional).Complete();
                    validation.ThrowIfHasError();
                }
                _empresaVendaService.Salvar(empresaVenda, validation);

                _regionalEmpresaService.Salvar(empresaVenda, idsRegionais);
                

                if (novaEmpresaVenda)
                {
                    var pontoVenda = new PontoVenda(empresaVenda);
                    _pontoVendaService.Salvar(pontoVenda, validation);
                }

                corretor.EmpresaVenda = empresaVenda;
                // Para preencher automaticamente os campos que não estão no formulário
                corretor.GerarCorretorCompleto();
                _corretorService.SalvarViaEmpresaVenda(corretor, validation);
                empresaVenda.Corretor = corretor;

                validation.ThrowIfHasError();

                _empresaVendaService.Salvar(empresaVenda, validation);

                if (documentos.HasValue())
                {
                    foreach (var doc in documentos)
                    {
                        doc.IdEmpresaVenda = empresaVenda.Id;
                        _documentoEmpresaVendaService.UploadDocumentoEmpresaVenda(doc);
                    }
                }

                json.Mensagens.Add(string.Format(GlobalMessages.MsgSucessoSalvarEmpresaVendaRepresentante,
                    empresaVenda.RazaoSocial, corretor.Nome));
                json.Sucesso = true;
                json.Objeto = new { IdEmpresaVenda = empresaVenda.Id, NomeEmpresaVenda = empresaVenda.NomeFantasia };
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS01", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Excluir(long idEmpresaVenda)
        {
            var json = new JsonResponse();
            var empresa = _empresaVendaRepository.FindById(idEmpresaVenda);
            try
            {
                _empresaVendaRepository.Delete(empresa);
                json.Sucesso = true;
                json.Mensagens.Add((String.Format(GlobalMessages.RegistroSucesso, empresa.ChaveCandidata(),
                    GlobalMessages.Excluido.ToLower())));
            }
            catch (GenericADOException ex)
            {
                json.Sucesso = false;
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    json.Mensagens.Add(String.Format(GlobalMessages.ErroViolacaoConstraint, empresa.ChaveCandidata()));
                }
                else
                {
                    json.Mensagens.Add(String.Format(GlobalMessages.ErroNaoTratado, ex.Message));
                }
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS01", "Reativar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Reativar(long[] idsEmpresas)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var usuario = SessionAttributes.Current().UsuarioPortal;
                var numAlterados = _empresaVendaService.AtivarEmLote(usuario, idsEmpresas.ToList());
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros,
                    GlobalMessages.Ativados.ToLower(), numAlterados, idsEmpresas.Count()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS01", "Suspender")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Suspender(long[] idsEmpresas)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var usuario = SessionAttributes.Current().UsuarioPortal;
                var numAlterados = _empresaVendaService.SuspenderEmLote(usuario, idsEmpresas.ToList());
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros,
                    GlobalMessages.Suspensos.ToLower(), numAlterados, idsEmpresas.Count()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS01", "Cancelar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Cancelar(long[] idsEmpresas)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var usuario = SessionAttributes.Current().UsuarioPortal;
                var numAlterados = _empresaVendaService.CancelarEmLote(usuario, idsEmpresas.ToList());
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros,
                    GlobalMessages.Cancelados.ToLower(), numAlterados, idsEmpresas.Count()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS01", "UploadFoto")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult UploadFoto(FotoEmpresaVendaDTO dto)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                _empresaVendaService.UploadFoto(dto.Foto, dto.IdEmpresaVenda);
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgUploadFotoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [BaseAuthorize("EVS01", "Visualizar")]
        public ActionResult BuscarEmpresaVenda(long idEmpresaVenda, long idCorretor)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var empresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
                var corretor = _corretorRepository.FindById(idCorretor);

                var result = new
                {
                    htmlEmpresaVenda = RenderRazorViewToString("_FormularioEmpresaVenda", empresaVenda, false),
                    htmlEnderecoEmpresaVenda = RenderRazorViewToString("_FormularioEndereco", empresaVenda, false),
                    htmlDadosTributarios = RenderRazorViewToString("_FormularioDadosTributarios", empresaVenda, false),
                    htmlRepresentanteLegal = RenderRazorViewToString("_FormularioDadosRepresentanteLegal", corretor, false),
                    htmlResponsavelTecnico = RenderRazorViewToString("_FormularioResponsavelTecnico", empresaVenda, false),
                    //verifica se a EV pode cadastrar reponsaveis pelo acite da regra comissao
                    podeCadastrarResponsavel = _documentoEmpresaVendaRepository.EvTemProcuracao(empresaVenda.Id),
                };
                jsonResponse.Objeto = result;
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgUploadFotoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS01", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, string empresaVenda, string cnpj,
            string creci, List<Situacao> situacoes, List<long> regionais,List<string> estados)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, empresaVenda, cnpj, creci, situacoes, regionais,estados);
            string nomeArquivo = GlobalMessages.EmpresaVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("EVS01", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, string empresaVenda, string cnpj,
            string creci, List<Situacao> situacoes, List<long> regionais, List<string> estados)
        {
            byte[] file = Exportar(request, empresaVenda, cnpj, creci, situacoes, regionais,estados);
            string nomeArquivo = GlobalMessages.EmpresaVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, string empresaVenda, string cnpj, string creci,
            List<Situacao> situacoes, List<long> regionais,List<string> estados)
        {
            var results = _viewEmpresaVendaRepository.Listar(empresaVenda, cnpj, creci, situacoes, regionais,estados)
                .ToDataRequest(request);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy";
            const string formatRG = "00.000.000-00";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.NomeFantasia).Width(20)
                    .CreateCellValue(model.RazaoSocial).Width(20)
                    .CreateCellValue(model.CNPJ.ToCNPJFormat()).Width(10)
                    .CreateCellValue(model.NomeLoja).Width(20)
                    .CreateCellValue(model.CreciJuridico).Width(10)
                    .CreateCellValue(model.CodigoFornecedorSap).Width(20)
                    .CreateCellValue(model.InscricaoMunicipal).Width(20)
                    .CreateCellValue(model.InscricaoEstadual).Width(20)
                    .CreateCellValue(model.NomeCorretor).Width(20)
                    .CreateCellValue(model.Telefone.ToPhoneFormat()).Width(20)
                    .CreateCellValue(model.Email).Width(25)
                    .CreateCellValue(model.RG).Width(20)
                    .CreateCellValue(model.CreciFisico).Width(20)
                    .CreateCellValue(model.CPF.ToCPFFormat()).Width(10)
                    .CreateCellValue(model.CnpjCorretor.ToCNPJFormat()).Width(20)
                    .CreateCellValue(model.DataNascimento.ToString(formatDate)).Width(20)
                    .CreateCellValue(model.LegislacaoFederal).Width(10)
                    .CreateCellValue(model.Simples).Width(20)
                    .CreateCellValue(model.Simei).Width(20)
                    .CreateMoneyCell(model.LucroPresumido).Width(15)
                    .CreateMoneyCell(model.LucroReal).Width(15)
                    .CreateCellValue(model.CEP.ToCEPFormat()).Width(15)
                    .CreateCellValue(model.Endereco).Width(30)
                    .CreateCellValue(model.Numero).Width(15)
                    .CreateCellValue(model.Complemento).Width(20)
                    .CreateCellValue(model.Bairro).Width(20)
                    .CreateCellValue(model.Cidade).Width(20)
                    .CreateCellValue(model.Estado).Width(15)
                    .CreateCellValue(model.Situacao.AsString()).Width(15)
                    .CreateDateTimeCell(model.CriadoEm).Width(15);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.NomeFantasia,
                GlobalMessages.RazaoSocial,
                GlobalMessages.Cnpj,
                GlobalMessages.CentralVendas,
                GlobalMessages.CreciJuridico,
                GlobalMessages.CodigoFornecedorSap,
                GlobalMessages.InscricaoMunicipal,
                GlobalMessages.InscricaoEstadual,
                GlobalMessages.RepresentanteLegal,
                GlobalMessages.Telefone,
                GlobalMessages.Email,
                GlobalMessages.RG,
                GlobalMessages.CreciFisico,
                GlobalMessages.Cpf,
                GlobalMessages.Cnpj,
                GlobalMessages.DataNascimento,
                GlobalMessages.LegislacaoFederal,
                GlobalMessages.Simples,
                GlobalMessages.Simei,
                GlobalMessages.LucroPresumido,
                GlobalMessages.LucroReal,
                GlobalMessages.CEP,
                GlobalMessages.Endereco,
                GlobalMessages.Numero,
                GlobalMessages.Complemento,
                GlobalMessages.Bairro,
                GlobalMessages.Cidade,
                GlobalMessages.Estado,
                GlobalMessages.Situacao,
                GlobalMessages.DataCriacao
            };
            return header.ToArray();
        }

        #region Pontuação fidelidade
        [HttpPost]
        public ActionResult ListarTreeEvs(long? idPontuacaoFidelidadeReferencia, string regional)
        {
            if (idPontuacaoFidelidadeReferencia.IsEmpty() && regional.IsEmpty())
            {
                return Json(new List<TreeViewModel>(), JsonRequestBehavior.AllowGet);
            }

            var empresas = _empresaVendaRepository.EmpresasDaRegional(regional).Where(x => x.Situacao == Situacao.Ativo);

            var empresasPontuacao = new List<long>();

            if (idPontuacaoFidelidadeReferencia.HasValue())
            {
                empresasPontuacao = _itemPontuacaoFidelidadeRepository.Queryable()
                    .Where(x => x.PontuacaoFidelidade.Id == idPontuacaoFidelidadeReferencia)
                    .Select(x => x.EmpresaVenda.Id)
                    .ToList();
            }

            var results = empresas.Select(x => new TreeViewModel
            {
                id = x.Id,
                text = x.NomeFantasia,
                selectable = true,
                state = new TreeViewState
                {
                    @checked = empresasPontuacao.Contains(x.Id),
                    disabled = false,
                    expanded = false,
                    selected = false
                }
            }).OrderBy(x => x.text);
            return Json(results.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Responsavel Aceite Regra Comissao

        [BaseAuthorize("EVS01", "Visualizar")]
        public ActionResult ListarResponsavelAceiteRegraComissao(DataSourceRequest request, RegraComissaoDto dto)
        {
            dto.DataSourceRequest = request;
            var result = _viewResponsavelAceiteRegraComissaoRepository.Listar(dto);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS01", "Suspender")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SuspenderResponsavelAceiteRegraComissao(RegraComissaoDto dto)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.SuspenderResponsavelAceiteRegraComissao(dto);
                result.Sucesso = true;
                result.Mensagens.AddRange(response.Messages);
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [BaseAuthorize("EVS01", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirResponsavelAceiteRegraComissao(RegraComissaoDto dto)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.IncluirResponsavelAceiteRegraComissao(dto);
                result.Sucesso = true;
                result.Mensagens.AddRange(response.Messages);
                result.Objeto = ((JObject)response.Data).ToObject<RegraComissaoDto>();
            }
            catch (ApiException e)
            {
                result.FromApiException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}