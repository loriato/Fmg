using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Tenda.Domain.Core.Data;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Api.Models;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [AuthenticateUserByToken]
    [ExceptionHandler]
    [ActionExecutingFilter]
    [Transaction(TransactionAttributeType.None)]
    public class BaseApiController : ApiController
    {
        public ISession _session { get; set; }
        public RequestStateApi RequestState { get; set; }
        public SimuladorApiService SimuladorApiService { get; set; }

        protected ISession Session
        {
            get { return _session; }
        }

        private ITransaction _transaction { get; set; }

        public RequestStateApi CurrentRequestState()
        {
            return RequestState;
        }

        public ISession CurrentSession()
        {
            return Session;
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public ITransaction CurrentTransaction()
        {
            return _transaction;
        }

        protected string GetAppRoot()
        {
            return Request.RequestUri.GetLeftPart(UriPartial.Authority) + Request.GetRequestContext().VirtualPathRoot;
        }

        protected string ToJson(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public HttpResponseMessage Response(object content)
        {
            return Response(HttpStatusCode.OK, ToJson(content));
        }

        public HttpResponseMessage Response(HttpStatusCode status, object content)
        {
            return Response(status, ToJson(content));
        }

        public HttpResponseMessage Response(HttpStatusCode status)
        {
            var response = new HttpResponseMessage(status);
            return response;
        }

        private HttpResponseMessage Response(HttpStatusCode status, string content)
        {
            var response = new HttpResponseMessage(status)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            return response;
        }

        public HttpResponseMessage NotFound(string entity, string key)
        {
            var message = string.Format(GlobalMessages.ResourceNotFound, entity, key);
            return NotFound(message);
        }

        public HttpResponseMessage NotFound(string message)
        {
            var errorResponse = new BaseResponse
            {
                Code = (int)HttpStatusCode.NotFound
            };
            errorResponse.Messages.Add(message);
            return Response(HttpStatusCode.NotFound, ToJson(errorResponse));
        }

        public HttpResponseMessage SendErrorResponse(HttpStatusCode status, ICollection<string> errors)
        {
            var errorResponse = new BaseResponse
            {
                Code = (int)status,
                Messages = errors?.ToList()
            };
            return Response(status, ToJson(errorResponse));
        }

        protected string GetWebAppRoot()
        {
            // Solução para o caso do site estar como HTTP no IIS e o BigIP que controla o HTTPS.
            // No PdfJs, o parametro é passado errado e ele não deixa o Host passar
            // https://stackoverflow.com/questions/37378251/load-pdf-on-foreign-url-with-pdf-js
            string parameterWebRoot = ProjectProperties.EvsBaseUrl;
            if (!parameterWebRoot.IsEmpty())
            {
                return parameterWebRoot;
            }

            var host = (Request.RequestUri.IsDefaultPort) ?
                Request.RequestUri.Host :
                Request.RequestUri.Authority;
            host = string.Format("{0}://{1}", Request.RequestUri.Scheme, host);
            if (Request.GetRequestContext().VirtualPathRoot == "/")
                return host;
            else
                return host + Request.GetRequestContext().VirtualPathRoot;
        }

        protected string GetWebAppRoot(string codigoSistema)
        {
            // Solução para o caso do site estar como HTTP no IIS e o BigIP que controla o HTTPS.
            // No PdfJs, o parametro é passado errado e ele não deixa o Host passar
            // https://stackoverflow.com/questions/37378251/load-pdf-on-foreign-url-with-pdf-js

            string parameterWebRoot = ProjectProperties.EvsBaseUrl;

            if (codigoSistema.Equals(ApplicationInfo.CodigoSistemaPortalHome))
            {
                parameterWebRoot = ProjectProperties.PortalHouseBaseUrl;
            }

            if (!parameterWebRoot.IsEmpty())
            {
                return parameterWebRoot;
            }

            var host = (Request.RequestUri.IsDefaultPort) ?
                Request.RequestUri.Host :
                Request.RequestUri.Authority;
            host = string.Format("{0}://{1}", Request.RequestUri.Scheme, host);
            if (Request.GetRequestContext().VirtualPathRoot == "/")
                return host;
            else
                return host + Request.GetRequestContext().VirtualPathRoot;
        }

        protected string GetWebAppRootAdmin()
        {
            // Solução para o caso do site estar como HTTP no IIS e o BigIP que controla o HTTPS.
            // No PdfJs, o parametro é passado errado e ele não deixa o Host passar
            // https://stackoverflow.com/questions/37378251/load-pdf-on-foreign-url-with-pdf-js
            string parameterWebRoot = ProjectProperties.EvsAdminBaseUrl;
            if (!parameterWebRoot.IsEmpty())
            {
                return parameterWebRoot;
            }
            var host = (Request.RequestUri.IsDefaultPort) ?
                Request.RequestUri.Host :
                Request.RequestUri.Authority;
            host = string.Format("{0}://{1}", Request.RequestUri.Scheme, host);
            if (Request.GetRequestContext().VirtualPathRoot == "/")
                return host;
            else
                return host + Request.GetRequestContext().VirtualPathRoot;
        }

        public void FromBusinessRuleException(BusinessRuleException bre)
        {
            var apiExc = new ApiException();
            apiExc.AddErrors(bre.Errors);
            apiExc.ThrowIfHasError();
        }
    }
}