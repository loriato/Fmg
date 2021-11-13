using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tenda.EmpresaVenda.PortalHouse.App_Start;
using Tenda.EmpresaVenda.PortalHouse.Commons;

namespace Tenda.EmpresaVenda.PortalHouse
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(typeof(SessionControllerFactory));
            AutofacConfig.ConfigureContainer();

            MessagesPublisher.Publish();
        }
    }
}
