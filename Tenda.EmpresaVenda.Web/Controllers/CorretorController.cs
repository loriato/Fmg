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
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{

    [BaseAuthorize("EVS02")]
    public class CorretorController : BaseController
    {
        private CorretorRepository _corretorRepository { get; set; }
        private ViewCorretorRepository _viewCorretorRepository { get; set; }
        private CorretorService _corretorService { get; set; }
        public SistemaRepository _sistemaRepository { get; set; }

        [BaseAuthorize("EVS02", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS02", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroCorretorDTO filtro)
        {

            var results = _viewCorretorRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarAutoComplete(DataSourceRequest request)
        {
            var results = _corretorService.Listar(request).Select(reg => new Corretor
            {
                Id = reg.Id,
                Nome = reg.Nome
            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarGerenteAutoComplete(DataSourceRequest request)
        {
            var results = _corretorService.Listar(request).Where(x => x.Funcao == TipoFuncao.Gerente).Select(reg => new Corretor
            {
                Id = reg.Id,
                Nome = reg.Nome
            });
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        private JsonResult Salvar(Corretor corretor)
        {
            var json = new JsonResponse();

            try
            {
                var validation = new BusinessRuleException();
                _corretorService.Salvar(corretor, validation);
                validation.ThrowIfHasError();

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, corretor.Nome));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS02", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(Corretor corretor)
        {
            return Salvar(corretor);
        }

        [BaseAuthorize("EVS02", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Atualizar(Corretor corretor)
        {
            return Salvar(corretor);
        }

        [HttpGet]
        [BaseAuthorize("EVS02", "Visualizar")]
        public ActionResult BuscarCorretor(long idCorretor)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var corretor = _corretorRepository.FindById(idCorretor);
                var result = new
                {
                    htmlCorretor = RenderRazorViewToString("_FormularioCorretor", corretor, false),
                };

                jsonResponse.Objeto = result;
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;

            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [BaseAuthorize("EVS02", "Ativar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Reativar(long[] idsCorretores)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;
                var numAlterados = _corretorService.AtivarEmLote(idUsuario, idsCorretores.ToList());
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros, GlobalMessages.Ativados.ToLower(), numAlterados, idsCorretores.Count()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;

            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS02", "Suspender")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Suspender(long[] idsCorretores)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;
                var numAlterados = _corretorService.SuspenderEmLote(idUsuario, idsCorretores.ToList());
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros, GlobalMessages.Suspensos.ToLower(), numAlterados, idsCorretores.Count()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;

            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS02", "Cancelar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Cancelar(long[] idsCorretores)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;
                var numAlterados = _corretorService.CancelarEmLote(idUsuario, idsCorretores.ToList());
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.AlteracaoDeRegistros, GlobalMessages.Cancelados.ToLower(), numAlterados, idsCorretores.Count()));
            }
            catch (Exception ex)
            {
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;

            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS02", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Excluir(long idCorretor)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            var entidade = _corretorRepository.FindById(idCorretor);

            try
            {
                _corretorService.ExcluirPorId(idCorretor);
                CurrentTransaction().Commit();
                json.Sucesso = true;
                json.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, entidade.Nome, GlobalMessages.Excluido.ToLower()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, entidade.ChaveCandidata()));
                    json.Sucesso = false;
                }
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS02", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]

        public ActionResult ReenviarTokenAtivacao(long idCorretor)
        {
            var jsonResponse = new JsonResponse();

            var corretor = _corretorRepository.FindById(idCorretor);

            _corretorService.CriarTokenAtivacaoEEnviarEmail(corretor);
            jsonResponse.Sucesso = true;
            jsonResponse.Mensagens.Add(String.Format(GlobalMessages.TokenAtivacaoReenviado, corretor.Nome));
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDaEmpresaVenda(DataSourceRequest request)
        {
            var result = _viewCorretorRepository.Listar(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscaEmpresaVendaPortal()
        {
            var json = new JsonResponse();
            try
            {
                var portal = _sistemaRepository.FindByCodigo(ProjectProperties.CodigoEmpresaVendaPortal);

                json.Sucesso = true;
                json.Objeto = new Sistema
                {
                    Id = portal.Id
                };
                json.Mensagens.Add(GlobalMessages.Sucesso);

            }
            catch (BusinessRuleException bre)
            {
                json.Mensagens.Add(GlobalMessages.Falha.ToString());
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [BaseAuthorize("EVS02", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroCorretorDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _corretorService.Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.Corretor;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        [BaseAuthorize("EVS02", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroCorretorDTO filtro)
        {
            byte[] file = _corretorService.Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.Corretor;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }


    }
}