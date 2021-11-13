using Autofac;
using Autofac.Core.NonPublicProperty;
using Autofac.Integration.Mvc;
using Europa.Rest;
using Europa.Web;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Data;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Integration.Zeus;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Rest;

namespace Tenda.EmpresaVenda.Web.App_Start
{
    public class AutofacConfig
    {

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Custom property injector to ready private properties
            builder.RegisterControllers(typeof(MvcApplication).Assembly).AutoWireNonPublicProperties();

            builder.RegisterModule(new AutofacWebTypesModule());

            builder.Register(x => SessionAttributes.Current()).As<ISessionAttributes>().InstancePerHttpRequest().AutoWireNonPublicProperties();

            builder.Register(x => new RequestState()).InstancePerHttpRequest().AutoWireNonPublicProperties();

            builder.Register(x => new DefaultInterceptor(ApplicationInfo.CodigoSistema, x.Resolve<RequestState>())).InstancePerRequest().AutoWireNonPublicProperties();

            builder.Register(x => NHibernateSession
                .Session(x.Resolve<DefaultInterceptor>()))
                .As<ISession>().InstancePerHttpRequest().AutoWireNonPublicProperties();

            builder.Register(x => NHibernateSession
             .StatelessSession())
             .As<IStatelessSession>().InstancePerHttpRequest().AutoWireNonPublicProperties();

            builder.RegisterInstance(new PerBaseUrlFlurlClientFactory()).SingleInstance().AutoWireNonPublicProperties();

            builder.Register(x => new EmpresaVendaApi(x.Resolve<PerBaseUrlFlurlClientFactory>())).SingleInstance()
                .AutoWireNonPublicProperties();

            JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.Default;

            FlurlHttp.Configure(settings => { settings.JsonSerializer = new NewtonsoftJsonSerializer(DefaultJsonSerializerSettings.Default); });

            //Zeus Api
            builder.Register(x => new ZeusApiService(x.Resolve<PerBaseUrlFlurlClientFactory>()))
                .SingleInstance()
                .AutoWireNonPublicProperties();
            
            //SEG
            Tenda.Domain.Security.AppStart.AutofacConfig.Register(builder);

            //App
            Tenda.EmpresaVenda.Domain.AppStart.AutofacConfig.Register(builder);


            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}