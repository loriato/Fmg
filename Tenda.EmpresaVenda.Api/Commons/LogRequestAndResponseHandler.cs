using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using Europa.Extensions;
using Europa.Web;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Api.Commons
{
    //TODO - Know Issue - Nathalia controllerType.GetMethod dispara referência ambigua quando existe métodos com mesmo nome no controller.
    //Orientar o uso de nomes diferentes nos métodos até achar uma solução
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Type controllerType = null;
            var actionName = string.Empty;

            GetControllerAndActionName(request, ref controllerType, ref actionName);

            var atribute = GetIgnoreRequestResponseLog(controllerType, actionName);

            var metadata = await LogRequestMetadata(request, atribute);

            // let other handlers process the request
            var response = await base.SendAsync(request, cancellationToken);

            metadata = await LogResponseMetadata(metadata, response, atribute);

            //FIXME: Request IP? Auth Header? Other Info?
            await PersistLog(metadata, atribute);

            return response;
        }

        private async Task<RequestResponseLog> LogRequestMetadata(HttpRequestMessage request,
            IgnoreRequestResponseLog atribute)
        {
            var metadata = new RequestResponseLog();
            if (atribute?.Type == IgnoreRequestResponseLogType.All ||
                atribute?.Type == IgnoreRequestResponseLogType.Request) return metadata;

            metadata.RequestMethod = request.Method.Method;
            metadata.RequestTimestamp = DateTime.Now;
            metadata.RequestUri = request.RequestUri.ToString();
            if (request.Content != null)
            {
                metadata.RequestBody = await request.Content.ReadAsStringAsync();
            }

            return metadata;
        }

        private async Task<RequestResponseLog> LogResponseMetadata(RequestResponseLog metadata,
            HttpResponseMessage response, IgnoreRequestResponseLog atribute)
        {
            if (atribute?.Type == IgnoreRequestResponseLogType.All ||
                atribute?.Type == IgnoreRequestResponseLogType.Response) return metadata;
            metadata.ResponseStatusCode = response.StatusCode;
            metadata.ResponseTimestamp = DateTime.Now;
            if (response.Content == null) return metadata;
            metadata.ResponseContentType = response.Content.Headers.ContentType.MediaType;
            metadata.ResponseBody = await response.Content.ReadAsStringAsync();
            return metadata;
        }

        private async Task PersistLog(RequestResponseLog logMetadata, IgnoreRequestResponseLog atribute)
        {
            if (atribute?.Type == IgnoreRequestResponseLogType.All) return;
            if (ProjectProperties.ApiConfiguration.EnableRequestResponseLog)
            {
                var session = NHibernateSession.StatelessSession();
                session.Insert(logMetadata);
                session.Close();
            }
        }

        private IgnoreRequestResponseLog GetIgnoreRequestResponseLog(Type controllerType, string actionName)
        {
            if (controllerType.IsEmpty() || actionName.IsEmpty()) return null;
            var method = controllerType.GetMethod(actionName);
            var attributeType = typeof(IgnoreRequestResponseLog);
            var attribute = method?.GetCustomAttributes(attributeType, false).OfType<IgnoreRequestResponseLog>()
                .FirstOrDefault();
            return attribute;
        }

        private void GetControllerAndActionName(HttpRequestMessage request, ref Type controllerType,
            ref string actionName)
        {
            try
            {
                var config = request.GetConfiguration();
                var routeData = config.Routes.GetRouteData(request);
                var controllerContext = new HttpControllerContext(config, routeData, request);

                request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
                controllerContext.RouteData = routeData;

                // get controller type
                var controllerDescriptor = new DefaultHttpControllerSelector(config).SelectController(request);
                controllerType = controllerDescriptor.ControllerType;
                controllerContext.ControllerDescriptor = controllerDescriptor;

                // get action name
                var actionMapping = new ApiControllerActionSelector().SelectAction(controllerContext);
                actionName = actionMapping.ActionName;
            }
            catch (Exception)
            {
                // ignored motivo swagger 
            }
        }
    }
}