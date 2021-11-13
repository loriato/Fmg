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
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Repository.Models;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;


namespace Tenda.EmpresaVenda.Portal.Controllers
{

    /// <summary>
    /// FIXME: Colocar proteções nas ações de Cancelar, Excluir, Ativar e Visualizar para nõa permitir ações cross EVS
    /// </summary>
    [BaseAuthorize("EVS02")]
    public class CorretorController : BaseController
    {
        private CorretorRepository _corretorRepository { get; set; }
        private ViewCorretorRepository _viewCorretorRepository { get; set; }
        private CorretorService _corretorService { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private PerfilService _perfilService { get; set; }
        public SistemaRepository _sistemaRepository { get; set; }
        private UsuarioPortalService _usuarioPortalService { get; set; }
        private PerfilRepository _perfilRepository { get; set; }

        [BaseAuthorize("EVS02", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS02", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroCorretorDTO filtro)
        {
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
            var results = _viewCorretorRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDaEmpresaVenda(DataSourceRequest request)
        {
            var result = _viewCorretorRepository.ListarDaEmpresaVenda(request, SessionAttributes.Current().EmpresaVendaId, SessionAttributes.Current().Corretor.Id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult ListarProprietariosEmpresaVenda(DataSourceRequest request)
        {
            var result = _viewCorretorRepository.ListarProprietariosEmpresaVenda(request, SessionAttributes.Current().EmpresaVendaId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarGerenteDaEmpresaVenda(DataSourceRequest request)
        {
            var result = _viewCorretorRepository.ListarGerenteDaEmpresaVenda(request, SessionAttributes.Current().EmpresaVendaId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private JsonResult Salvar(Corretor corretor)
        {
            var json = new JsonResponse();

            try
            {
                corretor.EmpresaVenda = _empresaVendaRepository.FindById(SessionAttributes.Current().EmpresaVendaId);

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
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, entidade.Nome));
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

        [HttpPost]
        [BaseAuthorize("EVS02", "IncluirPerfil")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarComPerfil(CorretorDTO corretor)
        {
            var json = new JsonResponse();

            try
            {
                corretor.NovoCorretor.EmpresaVenda = _empresaVendaRepository.FindById(SessionAttributes.Current().EmpresaVendaId);

                var validation = new BusinessRuleException();
                _corretorService.SalvarComPerfil(corretor, validation);
                validation.ThrowIfHasError();

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, corretor.NovoCorretor.Nome));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS02", "IncluirPerfil")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult IncluirPerfil(long idUsuario, long idPerfil, long idSistema)
        {
            var response = new JsonResponse();
            try
            {
                Sistema sistema = _sistemaRepository.FindById(idSistema);
                var obj = _usuarioPortalService.IncluirPerfil(idUsuario, idPerfil, sistema.Codigo);
                response.Objeto = obj;
                response.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, obj.Usuario.Login));
                response.Sucesso = true;
            }
            catch (BusinessRuleException e)
            {
                response.Mensagens = e.Errors.ToList();
            }

            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS02", "ExcluirPerfil")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult ExcluirPerfil(long idUsuario, long idPerfil, long idSistema)
        {
            var response = new JsonResponse();
            try
            {
                Sistema sistema = _sistemaRepository.FindById(idSistema);
                var obj = _usuarioPortalService.RemoverPerfil(idUsuario, idPerfil, sistema.Codigo);
                response.Objeto = obj;
                response.Mensagens.Add(string.Format(GlobalMessages.MsgExcluirSucesso, obj));
                response.Sucesso = true;
            }
            catch (BusinessRuleException e)
            {
                response.Mensagens = e.Errors.ToList();
            }

            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public JsonResult ListarPerfisPortalAutocomplete(DataSourceRequest request)
        {
            var result = _perfilService.ListarPerfisPortalAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaPerfisSelecionados(DataSourceRequest request, FiltroPerfilDTO filtro)
        {
            var result = _perfilService.ListaPerfisSelecionados(request, filtro);
            if (result.records.HasValue())
            {
                var resultadosSelecionados = result.records.Select(x => new Perfil
                {
                    Id = x.Id,
                    Nome = x.Nome
                });
                result.records = resultadosSelecionados.ToList();
            }
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

        [BaseAuthorize("EVS02", "BuscarPerfil")]
        public ActionResult BuscarPerfil(DataSourceRequest request, long id = 0)
        {
            var result = _perfilRepository.FindById(id);
            if (result.IsEmpty())
            {
                result = new Perfil();
                result.Nome = "";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [BaseAuthorize("EVS02", "Visualizar")]
        public ActionResult BuscarCorretorView(long idCorretor)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var corretor = _viewCorretorRepository.FindById(idCorretor);

                jsonResponse.Objeto = corretor;
                jsonResponse.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;

            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }
    }
}