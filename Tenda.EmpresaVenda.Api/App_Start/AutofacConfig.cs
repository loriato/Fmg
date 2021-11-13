using Autofac;
using Autofac.Core.NonPublicProperty;
using Autofac.Integration.WebApi;
using Europa.Rest;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using NHibernate;
using System.Reflection;
using System.Web.Http;
using Tenda.Domain.Core.Data;
using Tenda.EmpresaVenda.Api.Models;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Integration.Conecta;
using Tenda.EmpresaVenda.Domain.Integration.Simulador;

namespace Tenda.EmpresaVenda.Api
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            //Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).AutoWireNonPublicProperties();

            builder.RegisterInstance(new PerBaseUrlFlurlClientFactory()).SingleInstance().AutoWireNonPublicProperties();

            //Domain
            Domain.AppStart.AutofacConfig.Register(builder);

            //SEG
            Tenda.Domain.Security.AppStart.AutofacConfig.Register(builder);

            builder.Register(x => new RequestStateApi()).As<RequestStateApi>().InstancePerRequest()
                .AutoWireNonPublicProperties();
            // TODO: ver qual a melhor solução para o código do sistema
            builder.Register(x => new DefaultInterceptorApi(ApplicationInfo.CodigoSistema, x.Resolve<RequestStateApi>()))
                .InstancePerRequest().AutoWireNonPublicProperties();
            builder.Register(x => NHibernateSession.Session(x.Resolve<DefaultInterceptorApi>())).As<ISession>()
                .InstancePerRequest().AutoWireNonPublicProperties();

            builder.Register(x => Domain.Data.NHibernateSession
                .StatelessSession()).As<IStatelessSession>().InstancePerRequest().AutoWireNonPublicProperties();

            //Conecta
            builder.Register(x => new ConectaApiService(x.Resolve<PerBaseUrlFlurlClientFactory>()))
                .SingleInstance()
                .AutoWireNonPublicProperties();

            //Simulador
            builder.Register(x => new SimuladorApiService(x.Resolve<PerBaseUrlFlurlClientFactory>())).SingleInstance()
                .AutoWireNonPublicProperties();

            JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.Default;

            FlurlHttp.Configure(settings => { settings.JsonSerializer = new NewtonsoftJsonSerializer(DefaultJsonSerializerSettings.Default); });


            var container = builder.Build();

            // Set the dependency resolver for Web API.
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
        }
    }
}