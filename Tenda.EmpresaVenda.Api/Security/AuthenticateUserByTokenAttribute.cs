using Europa.Extensions;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Tenda.EmpresaVenda.Api.Cache;
using Tenda.EmpresaVenda.Api.Controllers;
using Tenda.EmpresaVenda.ApiService.Models;
using Tenda.EmpresaVenda.ApiService.Models.Funcionalidade;

namespace Tenda.EmpresaVenda.Api.Security
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AuthenticateUserByTokenAttribute : AuthorizationFilterAttribute
    {
        private readonly bool _dontNeedPermission;
        private readonly string _functionalityCommand;
        private readonly string _functionalUnitCode;
        private readonly bool _loggedUserRequired;
        private readonly bool _publicRequest;

        public AuthenticateUserByTokenAttribute()
        {
            _publicRequest = false;
            _loggedUserRequired = true;
            _dontNeedPermission = true;
            _functionalUnitCode = "";
            _functionalityCommand = "";
        }

        public AuthenticateUserByTokenAttribute(bool publicRequest)
        {
            _publicRequest = publicRequest;
            _loggedUserRequired = !publicRequest;
            _dontNeedPermission = publicRequest;
            _functionalUnitCode = "";
            _functionalityCommand = "";
        }

        public AuthenticateUserByTokenAttribute(bool loggedUserRequired, bool dontNeedPermission)
        {
            _loggedUserRequired = loggedUserRequired;
            _dontNeedPermission = dontNeedPermission;
            _publicRequest = false;
            _functionalUnitCode = "";
            _functionalityCommand = "";
        }

        public AuthenticateUserByTokenAttribute(string functionalUnitCode, string functionalityCommand)
        {
            _functionalUnitCode = functionalUnitCode;
            _functionalityCommand = functionalityCommand;
            _publicRequest = false;
            _loggedUserRequired = true;
            _dontNeedPermission = false;
        }

        public AuthenticateUserByTokenAttribute(string functionalUnitCode)
        {
            _functionalUnitCode = functionalUnitCode;
            _publicRequest = false;
            _loggedUserRequired = true;
            _dontNeedPermission = false;
            _functionalityCommand = "";
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var baseResponse = new BaseResponse();
            var returnCode = CanExecuteAction(actionContext, ref baseResponse);
            if (HttpStatusCode.OK == returnCode) return;
            var controller = (BaseApiController)actionContext.ControllerContext.Controller;
            var response = controller.Response(returnCode, baseResponse);
            actionContext.Response = response;
        }

        private HttpStatusCode CanExecuteAction(HttpActionContext actionContext, ref BaseResponse baseResponse)
        {
            //Executar sempre. Mesmo que o método seja público, o usuário logado poderá ser utilizado em algum contexto 
            var localizouUsuario = TryGetUser(actionContext);

            if (_publicRequest) return HttpStatusCode.OK;

            if (!localizouUsuario)
            {
                baseResponse.Code = (int)HttpStatusCode.Unauthorized;
                baseResponse.Messages.Add(GetHttpAuthorizationHeader(actionContext).IsEmpty()
                    ? "O token de autorização é obrigatório para acessar."
                    : "O acesso do usuário não é mais válido.");
                return HttpStatusCode.Unauthorized;
            }

            if (_dontNeedPermission) return HttpStatusCode.OK;

            var controller = (BaseApiController)actionContext.ControllerContext.Controller;
            var requestState = controller.CurrentRequestState();
            var temPermissao = PermissionCache.HasPermission(requestState.Perfis.Select(x => x.Id).ToList(),
                requestState.UnidadeFuncional, requestState.Funcionalidade, requestState.CodigoSistema);

            if (temPermissao)
            {
                return HttpStatusCode.OK;
            }

            baseResponse.Code = (int)HttpStatusCode.Forbidden;
            baseResponse.Messages.Add("Você não possui permissão para executar esta funcionalidade.");
            baseResponse.Data = new SemPermissaoDto
            { UnidadeFuncional = _functionalUnitCode, Funcionalidade = _functionalityCommand };
            return HttpStatusCode.Forbidden;
        }

        private string GetHttpAuthorizationHeader(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            var authKey = "";

            IEnumerable<string> httpHeaderAuth;
            if (!headers.TryGetValues("Authorization", out httpHeaderAuth)) return "";
            authKey = httpHeaderAuth.SingleOrDefault();
            return authKey;
        }

        /// <summary>
        ///     A partir do cabeçalho HTTP Authorization, vai tentar recuperar o usuário logado
        ///     E vai colocar ele no RequestState.
        /// </summary>
        private bool TryGetUser(HttpActionContext actionContext)
        {
            var authKey = GetHttpAuthorizationHeader(actionContext);
            if (authKey.IsEmpty()) return false;

            // Tenta buscar um acesso válido no cache.
            // FIXME: A função de cache tenta buscar um acesso do banco de dados. Não sei se deveria. Ao menos o provider de cache deveria ser separado
            var cacheDto = LoggedUserCache.CachedData(authKey);
            // O acesso não está no cache ou está inválido
            if (cacheDto == null) return false;

            if (actionContext.ControllerContext.Controller is BaseApiController)
            {
                var controller = actionContext.ControllerContext.Controller as BaseApiController;
                var requestState = controller.CurrentRequestState();
                requestState.Acesso = cacheDto.RequestState.Acesso;
                requestState.UsuarioPortal = cacheDto.RequestState.UsuarioPortal;
                requestState.Perfis = cacheDto.RequestState.Perfis;
                requestState.Funcionalidade = _functionalityCommand;
                requestState.UnidadeFuncional = _functionalUnitCode;
                requestState.CodigoSistema = cacheDto.RequestState.CodigoSistema;
            }

            return true;
        }
    }
}