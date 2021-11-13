using Europa.Commons;
using Europa.Resources;
using Europa.Rest;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Login;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;
using Tenda.EmpresaVenda.PortalHouse.Rest;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class LoginController : Controller
    {
        public EmpresaVendaApi EmpresaVendaApi { get; set; }
        public ActionResult Index()
        {
            if (SessionAttributes.Current().IsValidSession()) return RedirectToActionPermanent("Index", "Home");
            return View();
        }

        [HttpGet]
        public ActionResult Logar()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logar(LoginRequestDto viewModel)
        {
            try
            {
                GenericFileLogUtil.DevLogWithDateOnBegin("Login");
                viewModel.CodigoSistema = ApplicationInfo.CodigoSistema;

                var result = EmpresaVendaApi.Login(viewModel);

                GenericFileLogUtil.DevLogWithDateOnBegin("Result");

                var current = SessionAttributes.Current();
                current.LoginWithUser(result);
                //FIX - Comentado porque retorno não estava redirecionando
                if (result.PerfilInicial)
                {
                    ViewBag.PefilInicial = true;
                    return View("Index", viewModel);
                }

                GenericFileLogUtil.DevLogWithDateOnBegin("Perfil não pendente");

                FormsAuthentication.SetAuthCookie(result.Login, false);
                return RedirectToAction("Index", "Home");
            }
            catch (ApiException exc)
            {
                GenericFileLogUtil.DevLogWithDateOnBegin("Exceção");
                ExceptionLogger.LogException(exc);
                foreach(var msg in exc.GetResponse().Messages)
                {
                    if (msg.Equals(GlobalMessages.UsuarioOuSenhaIncorretos))
                    {
                        ViewBag.InvalidLogin = true;
                    }
                    GenericFileLogUtil.DevLogWithDateOnBegin(msg);
                    ModelState.AddModelError("login_failed", msg);
                }
                return View("Index", viewModel);
            }
        }

        public ActionResult Logout()
        {
            try
            {
                EmpresaVendaApi.Logout();
            }
            catch (Exception)
            {
                // ignored logar?
                // caso dê erro no logout da api (ex: token não mais válido), ignorar
            }

            var session = SessionAttributes.Current();
            session.Invalidate();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}