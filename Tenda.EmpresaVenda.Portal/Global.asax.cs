using Europa.Web;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tenda.EmpresaVenda.Domain.Cache;
using Tenda.EmpresaVenda.Portal.App_Start;
using Tenda.EmpresaVenda.Portal.Commons;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // To allow RequestState on Actions 
            ControllerBuilder.Current.SetControllerFactory(typeof(SessionControllerFactory));

            AutofacConfig.ConfigureContainer();

            MessagesPublisher.Publish();

            ModelBinders.Binders.Add(typeof(string), new TrimModelBinder());
            ModelBinders.Binders[typeof(double)] = new DoubleModelBinder();
            ModelBinders.Binders[typeof(double?)] = new NullableDoubleModelBinder();

            ApplicationInfo.ConfigurarModoExecucao();
        }
    }
}
