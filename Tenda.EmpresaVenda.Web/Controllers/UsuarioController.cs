using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Controllers;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Models.PreferenciasUsuarioViewModel;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.Domain.Core.Services
{
    [BaseAuthorize("SEG03")]
    public class UsuarioController : BaseController
    {
        private ViewUsuarioPerfilRepository _viewUsuarioPerfilRepository { get; set; }
        private UsuarioPortalService _usuarioPortalService { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private PerfilService _perfilService { get; set; }
        public SistemaRepository _sistemaRepository { get; set; }
        public SupervisorViabilizadorService _supervisorViabilizadorService { get; set; }
        private HierarquiaHouseService _hierarquiaHouseService { get; set; }

        public UsuarioController(ISession session) : base(session)
        {
        }

        [BaseAuthorize("SEG03", "Visualizar")]
        public ActionResult Index()
        {
            PreferenciasUsuarioViewModel dto = new PreferenciasUsuarioViewModel();
            dto.Sistema = new Sistema();
            dto.Sistemas = _sistemaRepository.Queryable().OrderBy(x => x.Codigo).ToList();
            return View(dto);
        }

        public ActionResult Listar(DataSourceRequest request, string nameOrEmail, string[] tipos, string name, string login, string email, long? perfil, long idSistema)
        {
            Sistema sistema = _sistemaRepository.FindById(idSistema);
            var query = _viewUsuarioPerfilRepository.Listar(request, nameOrEmail, tipos, name, login, email, perfil, sistema.Codigo);

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTodos(DataSourceRequest request)
        {
            var query = _viewUsuarioPerfilRepository.ListarTodos(request);

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarPerfis(DataSourceRequest request, long idUsuario, long idSistema)
        {
            Sistema sistema = _sistemaRepository.FindById(idSistema);
            var result = _usuarioPortalService.ListarPerfisUsuario(request, idUsuario, sistema.Codigo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Salvar(UsuarioViewModel model)
        {
            var response = new JsonResponse();
            try
            {
                response.Objeto = _usuarioPortalService.Salvar(model.Usuario, ApplicationInfo.CodigoSistema);
                response.Sucesso = true;
            }
            catch (BusinessRuleException e)
            {
                response.Mensagens = e.Errors.ToList();
            }

            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [BaseAuthorize("SEG03", "ExcluirPerfil")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult ExcluirPerfil(long idUsuario, long idPerfil, long idSistema)
        {
            var response = new JsonResponse();
            try
            {
                Sistema sistema = _sistemaRepository.FindById(idSistema);
                var usuarios = new List<long>{ idUsuario };
                _supervisorViabilizadorService.ExcluirRelacaoCoordenadorSupervisorViabilizador(usuarios,idPerfil);
                var obj = _usuarioPortalService.RemoverPerfil(idUsuario, idPerfil, sistema.Codigo);
                _hierarquiaHouseService.RemoverPerfil(idUsuario, idPerfil);
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

        [HttpPost]
        [BaseAuthorize("SEG03", "IncluirPerfil")]
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

        [BaseAuthorize("SEG03", "Incluir")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Incluir(UsuarioViewModel model)
        {
            return Salvar(model);
        }

        [BaseAuthorize("SEG03", "Alterar")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Atualizar(UsuarioViewModel model)
        {
            return Salvar(model);
        }

        [BaseAuthorize("SEG03", "Visualizar")]
        [HttpGet]
        public ActionResult GetUser(long id)
        {
            var portalUser = _usuarioPortalRepository.FindById(id);


            return Json(portalUser, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [BaseAuthorize("SEG03", "Cancelar")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Cancelar(long[] ids)
        {
            return ChangeSitu(ids, SituacaoUsuario.Cancelado);
        }

        [HttpPost]
        [BaseAuthorize("SEG03", "Suspender")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Suspender(long[] ids)
        {
            var action = ChangeSitu(ids, SituacaoUsuario.Suspenso);
            _supervisorViabilizadorService.ExcluirRelacaoCoordenadorSupervisorViabilizador(ids);
            return action;
        }

        [HttpPost]
        [BaseAuthorize("SEG03", "Ativar")]
        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult Ativar(long[] ids)
        {
            return ChangeSitu(ids, SituacaoUsuario.Ativo);
        }

        [TransactionAttribute(TransactionAttributeType.Required)]
        public ActionResult ChangeSitu(long[] ids, SituacaoUsuario situ)
        {
            var response = new JsonResponse { Sucesso = true };
            _usuarioPortalService.AlterarSitu(ids, situ);
            return Json(response);
        }

        public ActionResult BuscarUsuarioPortal(DataSourceRequest request, long id = 0)
        {
            var result = _usuarioPortalRepository.FindById(id);
            if (result.IsEmpty())
            {
                result = new UsuarioPortal();
                result.Nome = "";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG03", "ExportarUsuariosAtivos")]
        public FileContentResult ExportarUsuariosAtivos(DataSourceRequest request, FiltroUsuariosDTO filter)
        {
            Sistema sistema = _sistemaRepository.FindById(filter.idSistema);
            request.pageSize = 0;
            request.start = 0;
            var results = _usuarioPortalService.ExportarUsuariosAtivos(request, filter.nameOrEmail, filter.tipos, filter.name, filter.login, filter.email, filter.perfil, sistema.Codigo);
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(results, MimeMappingWrapper.Xlsx, $"RelatorioUsuariosAtivos_{date}.xlsx"); ;
        }
    }
}