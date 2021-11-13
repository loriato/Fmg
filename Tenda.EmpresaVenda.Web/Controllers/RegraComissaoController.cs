using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;
using RegraComissaoDTO = Tenda.EmpresaVenda.Web.Models.RegraComissaoDTO;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS14")]
    public class RegraComissaoController : BaseController
    {
        private ViewRegraComissaoRepository _viewRegraComissaoRepository { get; set; }
        private ViewAceiteRegraComissaoRepository _viewAceiteRegraComissaoRepository { get; set; }
        private RegraComissaoRepository _regraComissaoRepository { get; set; }
        private RegraComissaoService _regraComissaoService { get; set; }
        private ContratoCorretagemRepository _contratoCorretagemRepository { get; set; }
        private ContratoCorretagemService _contratoCorretagemService { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private RegraComissaoPdfService _regraComissaoPdfService { get; set; }
        private ViewRegraComissaoEvsRepository _viewRegraComissaoEvsRepository { get; set; }
        private ViewRegraComissaoEvsService _viewRegraComissaoEvsService { get; set; }

        [BaseAuthorize("EVS14", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS14", "Visualizar")]
        public ActionResult Detalhar(long? id)
        {
            RegraComissao regra = new RegraComissao();
            if (!id.IsEmpty())
            {
                regra = _regraComissaoRepository.FindById(id.Value);
                if (regra.IsEmpty())
                {
                    RouteData.Values.Remove("id");
                    return RedirectToAction("Detalhar");
                }
            }

            return View("AceiteRegraComissao", regra);
        }

        [BaseAuthorize("EVS14", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, FiltroRegraComissaoDTO filtro)
        {
            var result = _viewRegraComissaoEvsRepository.Listar(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS14", "Visualizar")]
        public JsonResult ListarAceite(DataSourceRequest request, long? idRegra)
        {
            var result = _viewAceiteRegraComissaoRepository.ListarPorRegraComissao(idRegra.Value);
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS14", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Incluir(RegraComissaoDTO model)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();
            try
            {
                RegraComissao regra = new RegraComissao();
                regra.Descricao = model.Descricao;
                regra.Regional = model.Regional;
                regra.Situacao = SituacaoRegraComissao.Rascunho;

                _regraComissaoService.Salvar(regra, model.Arquivo, bre);

                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, regra.ChaveCandidata(),
                    GlobalMessages.Incluido.ToLower()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS14", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Atualizar(RegraComissaoDTO model)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();
            RegraComissao regra = new RegraComissao();
            try
            {
                regra = _regraComissaoRepository.FindById(model.Id.Value);
                if (regra.IsEmpty())
                {
                    bre.AddError(GlobalMessages.RegraComissaoNaoEncontrada).Complete();
                }

                if (model.Descricao.IsEmpty())
                {
                    bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao))
                        .AddField("Descricao").Complete();
                }

                bre.ThrowIfHasError();

                regra.Descricao = model.Descricao;
                _regraComissaoRepository.Save(regra);

                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, regra.ChaveCandidata(),
                    GlobalMessages.Atualizado.ToLower()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS14", "Liberar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Liberar(long idRegra)
        {
            var json = new JsonResponse();
            var responsavel = SessionAttributes.Current().UsuarioPortal;
            try
            {
                var regra = _regraComissaoRepository.FindById(idRegra);

                if (regra.Tipo == TipoRegraComissao.Campanha)
                {
                    _regraComissaoService.LiberarCampanha(regra,responsavel);
                }
                else
                {
                    _regraComissaoService.Liberar(regra,responsavel);
                }
                         
                var msg = regra.Tipo == TipoRegraComissao.Campanha ?
                    string.Format("A {0} {1} está {2}",GlobalMessages.TipoRegraComissao_Campanha,regra.ChaveCandidata(),GlobalMessages.SituacaoRegraComissao_AguardandoLiberacao)
                    : string.Format(GlobalMessages.RegistroLiberadoComSucesso, regra.ChaveCandidata());
                json.Mensagens.Add(msg);
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS14", "IncluirContratoCorretagem")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarContratoCorretagem(HttpPostedFileBase file)
        {
            var result = new JsonResponse();
            BusinessRuleException bre = new BusinessRuleException();
            try
            {
                if (file == null)
                {
                    bre.AddError(GlobalMessages.SelecioneArquivoParaEnviar).Complete();
                }
                else if (file.ContentType != "application/pdf")
                {
                    bre.AddError(string.Format(GlobalMessages.MsgUploadFormatoInvalido, GlobalMessages.PDF)).Complete();
                }

                bre.ThrowIfHasError();

                var contrato = _contratoCorretagemService.SalvarContratoCorretagem(file);

                result.Sucesso = true;
                var viewModel = RenderRazorViewToString("_ContratoCorretagem", contrato, false);
                result.Objeto = viewModel;
                //result.Objeto = contrato;
                result.Mensagens.Add(string.Format(GlobalMessages.RegistroIncluidoOuAlteradoSucesso,
                    contrato.NomeDoubleCheck, GlobalMessages.Incluido));
            }
            catch (BusinessRuleException exc)
            {
                result.Sucesso = false;
                result.Mensagens.AddRange(exc.Errors);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [BaseAuthorize("EVS14", "Visualizar")]
        public JsonResult RenderContratoCorretagem()
        {
            var contrato = _contratoCorretagemRepository.BuscarContratoMaisRecente();
            var viewModel = RenderRazorViewToString("_ContratoCorretagem", contrato, false);
            var result = new JsonResponse();
            result.Sucesso = true;
            result.Objeto = viewModel;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS14", "DownloadContratoCorretagem")]
        public FileContentResult DownloadContratoCorretagem()
        {
            var contrato = _contratoCorretagemRepository.BuscarContratoMaisRecenteComArquivo();
            return File(contrato.Arquivo.Content, contrato.Arquivo.ContentType, contrato.Arquivo.Nome);
        }

        [BaseAuthorize("EVS14", "DownloadRegraComissao")]
        public FileContentResult DownloadRegraComissao(long idRegra)
        {
            var regra = _regraComissaoRepository.FindById(idRegra);
            return File(regra.Arquivo.Content, regra.Arquivo.ContentType, regra.Arquivo.Nome);
        }

        private RegraComissao MontaRegraComArquivo(RegraComissaoDTO model)
        {
            RegraComissao regra = new RegraComissao();
            return regra;
        }

        [BaseAuthorize("EVS14", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, long idRegraComissao)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            var regraComissao = _regraComissaoRepository.FindById(idRegraComissao);

            byte[] file = _regraComissaoService.Exportar(request, idRegraComissao);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{regraComissao.Descricao} - {regraComissao.Regional} - {regraComissao.InicioVigencia.Value.ToDate()}.xlsx");
        }

        [BaseAuthorize("EVS14", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, long idRegraComissao)
        {
            var regraComissao = _regraComissaoRepository.FindById(idRegraComissao);
            byte[] file = _regraComissaoService.Exportar(request, idRegraComissao);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{regraComissao.Descricao} - {regraComissao.Regional} - {regraComissao.InicioVigencia.Value.ToDate()}.xlsx");
        }

        [HttpGet]
        public JsonResult ListarEmpresaVendasRegraComissao(long idRegraComissao)
        {
            var empresas = _regraComissaoEvsRepository.BuscarPorRegraComissao(idRegraComissao)
                .Select(x => new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda
                {
                    Id = x.EmpresaVenda.Id,
                    NomeFantasia = x.EmpresaVenda.NomeFantasia
                });

            return Json(empresas, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS14", "ExportarPagina")]
        public FileContentResult ExportarRegrasComissaoEvsPagina(DataSourceRequest request, FiltroRegraComissaoDTO filtro)
        {
            byte[] file = _viewRegraComissaoEvsService.Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.RegraComissaoEvs;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("EVS14", "ExportarTodos")]
        public FileContentResult ExportarRegrasComissaoEvsTodos(DataSourceRequest request, FiltroRegraComissaoDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _viewRegraComissaoEvsService.Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.RegraComissaoEvs;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("EVS14", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ExcluirPorId(long idRegra)
        {
            var json = new JsonResponse();
            var regra = _regraComissaoRepository.FindById(idRegra);
            try
            {   
                _regraComissaoService.ExcluirPorId(regra);

                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSucesso, regra.ChaveCandidata(),
                    GlobalMessages.Excluido.ToLower()));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, regra.ChaveCandidata()));
                    json.Sucesso = false;
                }
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [BaseAuthorize("EVS14", "InterromperCampanha")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult InterromperCampanha(long idRegra)
        {
            var result = new JsonResponse();

            try
            {
                _regraComissaoService.InterromperCampanha(idRegra);
                result.Sucesso = true;
                result.Mensagens.Add(GlobalMessages.SucessoInterromperCampanha);

            }catch(BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                result.Sucesso = false;
                result.Mensagens.AddRange(bre.Errors);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}