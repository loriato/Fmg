using Europa.Fmg.Portal.App_Start;
using Europa.Fmg.Portal.Commons;
using Europa.Fmg.Portal.Models.Application;
using Europa.Web;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Europa.Fmg.Domain.Cache;

namespace Europa.Fmg.Portal
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
