using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Security
{
    public class BaseAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly bool _acessivelParaUsuarioLogado;
        private readonly string _codigoUnidadeFuncional;
        private readonly string _comandoFuncionalidade;

        public BaseAuthorizeAttribute()
        {
        }

        public BaseAuthorizeAttribute(bool acessivelParaUsuarioLogado)
        {
            _acessivelParaUsuarioLogado = acessivelParaUsuarioLogado;
        }

        public BaseAuthorizeAttribute(string codigoUnidadeFuncional)
        {
            _codigoUnidadeFuncional = codigoUnidadeFuncional;
        }

        public BaseAuthorizeAttribute(string codigoUnidadeFuncional, string comandoFuncionalidade)
        {
            _codigoUnidadeFuncional = codigoUnidadeFuncional;
            _comandoFuncionalidade = comandoFuncionalidade;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            //Esta nas paginas de login
            if (user != null && user.Identity.IsAuthenticated)
            {
                var currentUser = filterContext.HttpContext.User.Identity.Name;
                var currentSession = SessionAttributes.Current();

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
                else if (!_codigoUnidadeFuncional.IsEmpty() && _comandoFuncionalidade.IsEmpty() &&
                         !currentSession.HasPermission(_codigoUnidadeFuncional, _comandoFuncionalidade))
                {
                    //Tem acesso
                }
                else if (currentSession.HasPermission(_codigoUnidadeFuncional, _comandoFuncionalidade))
                {
                    //Tem acesso
                }
                else
                {
                    throw new UnauthorizedPermissionException(GlobalMessages.AcessoNegado, _codigoUnidadeFuncional,
                        _comandoFuncionalidade);
                }
            }
            else
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    var result = new JavaScriptResult { Script = "document.location.reload()" };
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
}