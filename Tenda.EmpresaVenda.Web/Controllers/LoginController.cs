using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class LoginController : Controller
    {
        private SistemaService _sistemaService { get; set; }
        private MenuService _menuService { get; set; }
        private LoginService _loginService { get; set; }
        private PermissaoService _permissaoService { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private ParametroSistemaRepository _parametroSistemaRepository { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }

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
        [ValidateAntiForgeryToken]
        public ActionResult Logar(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            try
            {
                if (viewModel.Username.Contains("@"))
                {
                    throw new BusinessRuleException(GlobalMessages.UsuarioOuSenhaInvalidos + ". " + GlobalMessages.UseUsuarioTenda);
                }

                LoginDto loginDto = new LoginDto();
                loginDto.ClienteIpAddress = HttpUtil.RequestIp(Request);
                loginDto.Server = Environment.MachineName;
                loginDto.UserAgent = Request.UserAgent;
                loginDto.Username = viewModel.Username.ToLower();
                loginDto.CodigoSistema = ApplicationInfo.CodigoSistema;
                loginDto.Password = viewModel.Password;
                loginDto.LoginViaActiveDirectory = ProjectProperties.LoginViaActiveDirectory;
                loginDto.SincronizarGruposActiveDirectory = ProjectProperties.SincronizarGruposActiveDirectory;

                var acesso = _loginService.Login(loginDto);

                viewModel.Password = null;

                if (acesso == null || acesso.Usuario == null)
                {
                    ModelState.AddModelError("login_failed", GlobalMessages.UsuarioOuSenhaInvalidos);
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

                    var perfis = _loginService.GetPerfisFromAcesso(acesso.Id);
                    var idPerfis = perfis.Select(reg => reg.Id).ToList();

                    current.LoginWithUser(usuarioPortal, perfis, acesso);
                    current.Menu = _menuService.MontarMenuPerfil(ApplicationInfo.CodigoSistema, idPerfis);
                    current.Permissoes = _permissaoService.Permissoes(ApplicationInfo.CodigoSistema, idPerfis);

                    MenuItemToBootStrap menuConverter = new MenuItemToBootStrap();
                    string urlAcesso = GetWebAppRoot();
                    current.MenuHtml = menuConverter.ProcessMenu(urlAcesso, current.Menu);
                    current.NotificacoesNaoLidas = _notificacaoRepository.NaoLidasDoUsuario(usuarioPortal.Id, usuarioPortal.UltimaLeituraNotificacao, DateTime.Now).Count();

                    FormsAuthentication.SetAuthCookie(viewModel.Username, false);

                    if (UsuarioNoPerfilInicial(idPerfis))
                    {
                        return RedirectToAction("SemAcesso", "Home");
                    }
                    if (viewModel.ReturnUrl.IsNull())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(viewModel.ReturnUrl);
                }
                else if (SituacaoUsuario.PendenteAprovacao.Equals(acesso.Usuario.Situacao))
                {
                    ModelState.AddModelError("login_failed", GlobalMessages.CadastroPendenteAprovacao);
                }
                else
                {
                    ModelState.AddModelError("login_failed", GlobalMessages.UsuarioBloqueado);
                }
            }
            catch (COMException activeDirectoryException)
            {
                ModelState.AddModelError("login_failed", GlobalMessages.LoginActiveDirectoryInalcancavel);
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

        [HttpPost]
        public ActionResult Logout()
        {
            //BaseAuthorizeAttribute.SetSecurityParameters("SEG01", "Logout");
            SessionAttributes session = SessionAttributes.Current();
            _loginService.Logout(session.Acesso);
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