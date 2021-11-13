using Europa.Rest;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Commons;
using Tenda.EmpresaVenda.Api.Security;

namespace Tenda.EmpresaVenda.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ExceptionHandlerAttribute());
            config.Filters.Add(new AuthenticateUserByTokenAttribute());

            config.MessageHandlers.Add(new LogRequestAndResponseHandler());

            JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.Default;

            FlurlHttp.Configure(settings =>
            {
                settings.JsonSerializer = new NewtonsoftJsonSerializer(DefaultJsonSerializerSettings.Default);
            });

            config.EnsureInitialized();           
            
        }
    }
}
