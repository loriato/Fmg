using Europa.Commons;
using Europa.Extensions;
using Europa.Fmg.Portal.Commons;
using Europa.Fmg.Portal.Models;
using Europa.Fmg.Portal.Models.Application;
using Europa.Fmg.Portal.Security;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Shared;

namespace Europa.Fmg.Portal.Controllers
{
    public class LoginController : Controller
    {
        private MenuService _menuService { get; set; }
        private LoginService _loginService { get; set; }
        private PermissaoService _permissaoService { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private ParametroSistemaRepository _parametroSistemaRepository { get; set; }

        public ActionResult Index()
        {
            if (SessionAttributes.Current().IsValidSession())
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpGet]
        public ActionResult Logar()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Logar(LoginViewModel viewModel)
        {
            try
            {
                LoginDto loginDto = new LoginDto();
                loginDto.ClienteIpAddress = HttpUtil.RequestIp(Request);
                loginDto.Server = Environment.MachineName;
                loginDto.UserAgent = Request.UserAgent;
                loginDto.Username = viewModel.Username.ToLower();
                loginDto.CodigoSistema = ApplicationInfo.CodigoSistema;
                loginDto.Password = viewModel.Password;
                // A gestão de usuários do portal é realizado e mantido pela área de negócios, não por SI
                loginDto.LoginViaActiveDirectory = false;
                loginDto.SincronizarGruposActiveDirectory = false;

                var acesso = _loginService.Login(loginDto);

                viewModel.Password = null;

                if (acesso == null || acesso.Usuario == null)
                {
                    ModelState.AddModelError("login_failed", GlobalMessages.UsuarioSenhaIncorreto);
                }
                else if (SituacaoUsuario.Ativo.Equals(acesso.Usuario.Situacao))
                {
                    SessionAttributes current = SessionAttributes.Current();

                    UsuarioPortal usuarioPortal = _usuarioPortalRepository.FindById(acesso.Usuario.Id);

                    // Usado para criar os usuários automaticamente no primeiro login
                    if (usuarioPortal == null)
                    {
                        usuarioPortal = new UsuarioPortal(acesso.Usuario);
                        _usuarioPortalRepository.Save(usuarioPortal);
                    }

                    current.LoginWithUser(usuarioPortal);

                    string urlAcesso = GetWebAppRoot();
                    FormsAuthentication.SetAuthCookie(viewModel.Username, false);

                }

                return RedirectToAction("Index", "Home");

            }
            catch (BusinessRuleException bre)
            {
                ModelState.AddModelError("login_failed", bre.Errors.First());
            }
            return View("Index", viewModel);

        }

        private bool UsuarioNoPerfilInicial(List<long> idPerfis)
        {
            var perfilInicial = _parametroSistemaRepository.Queryable()
                .Where(reg => reg.Sistema.Codigo == ApplicationInfo.CodigoSistema)
                .Select(reg => reg.PerfilInicial)
                .SingleOrDefault();

            if (perfilInicial == null)
            {
                return false;
            }
            return idPerfis.TrueForAll(x => x == perfilInicial.Id);
        }

        public ActionResult Logout()
        {
            SessionAttributes session = SessionAttributes.Current();

            _loginService.Logout();
            session.Invalidate();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
        private string GetWebAppRoot()
        {
            var host = (Request.Url.IsDefaultPort) ?
                Request.Url.Host :
                Request.Url.Authority;
            host = string.Format("{0}://{1}", Request.Url.Scheme, host);
            if (Request.ApplicationPath == "/")
                return host;
            else
                return host + Request.ApplicationPath;
        }
    }
}