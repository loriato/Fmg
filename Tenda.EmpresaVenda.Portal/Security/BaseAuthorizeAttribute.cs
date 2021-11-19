using Europa.Commons;
using Europa.Extensions;
using NHibernate.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Tenda.EmpresaVenda.Portal.Controllers;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal.Security
{

    public class BaseAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _codigoUnidadeFuncional;
        private readonly string _comandoFuncionalidade;
        private readonly bool _acessivelParaUsuarioLogado;

        public BaseAuthorizeAttribute() { }

        public BaseAuthorizeAttribute(bool acessivelParaUsuarioLogado)
        {
            _acessivelParaUsuarioLogado = acessivelParaUsuarioLogado;
        }

        public BaseAuthorizeAttribute(string codigoUnidadeFuncional)
        {
            this._codigoUnidadeFuncional = codigoUnidadeFuncional;
        }

        public BaseAuthorizeAttribute(string codigoUnidadeFuncional, string comandoFuncionalidade)
        {
            this._codigoUnidadeFuncional = codigoUnidadeFuncional;
            this._comandoFuncionalidade = comandoFuncionalidade;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (PublicPages.IsPublicPage(filterContext))
            {
                return;
            }

            // Setando Request Context
            if (_comandoFuncionalidade.HasValue())
            {
                var controller = filterContext.Controller;
                if (controller is BaseController)
                {
                    var requestState = ((BaseController)controller)._requestState;
                    requestState.Funcionalidade = _comandoFuncionalidade;
                    requestState.UnidadeFuncional = _codigoUnidadeFuncional;
                }
            }

            var user = filterContext.HttpContext.User;
            //Esta nas paginas de login
            if (user != null && user.Identity.IsAuthenticated)
            {
                string currentUser = filterContext.HttpContext.User.Identity.Name;
                SessionAttributes currentSession = SessionAttributes.Current();

                //Validando a sessao do usuario logado
                if (!currentSession.IsValidSession(currentUser))
                {
                    SessionAttributes.Current().Invalidate();
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else if (_acessivelParaUsuarioLogado)
                {
                    //A funcionalidade é liberada para todo o usuário logado. 
                }

                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                            new
                            {
                                controller = "Home",
                                action = "AcessoNegado",
                                uf = _codigoUnidadeFuncional,
                                funcionalidade = _comandoFuncionalidade,
                                isAjax = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest()
                            }));
                }
            }
            else
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    JavaScriptResult result = new JavaScriptResult { Script = "document.location.reload()" };
                    filterContext.Result = result;
                }
                else
                {
                    //O código de FormsAuthentication.RedirectToLoginPage() não interrompe o fluxo.
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Login" }));
                }
            }
        }
    }

    public static class PublicPages
    {
        private static IList<string> _publicPages = new List<string>();
        public static bool IsPublicPage(AuthorizationContext filterContext)
        {
            PublicPage publicPage = (PublicPage)filterContext.ActionDescriptor.GetCustomAttributes(typeof(PublicPage), false).FirstOrDefault();
            if (publicPage != null)
            {
                return true;
            }

            if (_publicPages == null || _publicPages.IsEmpty())
            {
                if (filterContext.HttpContext.Request.Url != null)
                {
                    var segment = "";
                    if (filterContext.HttpContext.Request.Url.Segments.Length >= 2)
                    {
                        segment = filterContext.HttpContext.Request.Url.Segments[1].ToLower();
                    }
                    if (!segment.IsEmpty() && segment.Last() != '/')
                    {
                        segment += '/';
                    }
                    if (!segment.IsEmpty() && segment.First() != '/')
                    {
                        segment += "/" + segment;
                    }
                    RegisterPublicPages(segment);
                }
            }
            var canAccess = _publicPages != null && _publicPages.Contains(filterContext.HttpContext.Request.Path.ToLower());
            return canAccess;
        }

        private static void RegisterPublicPages(string applicationPreffix)
        {
            _publicPages = new List<string>
            {
                FormsAuthentication.LoginUrl.ToLower(),
                "login",
                "login/index"
            };
        }
    }
}
