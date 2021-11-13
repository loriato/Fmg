using System.Web.Mvc;
using Autofac;
using Autofac.Core.NonPublicProperty;
using Autofac.Integration.Mvc;
using Europa.Rest;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;
using Tenda.EmpresaVenda.PortalHouse.Rest;

namespace Tenda.EmpresaVenda.PortalHouse
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Custom property injector to ready private properties
            builder.RegisterControllers(typeof(MvcApplication).Assembly).AutoWireNonPublicProperties();

            builder.RegisterModule(new AutofacWebTypesModule());

            builder.RegisterInstance(new PerBaseUrlFlurlClientFactory()).SingleInstance().AutoWireNonPublicProperties();

            builder.Register(x => new EmpresaVendaApi(x.Resolve<PerBaseUrlFlurlClientFactory>())).SingleInstance()
                .AutoWireNonPublicProperties();

            JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings.Default;

            FlurlHttp.Configure(settings => { settings.JsonSerializer = new NewtonsoftJsonSerializer(DefaultJsonSerializerSettings.Default); });

            var container = builder.Build();

            var autofacResolver = new AutofacDependencyResolver(container);

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(autofacResolver);

            //Injetando o empresa venda api no ApplicationInfo - solução para estático
            ApplicationInfo.EmpresaVendaApi = autofacResolver.GetService<EmpresaVendaApi>();
        }
    }
}