using System.Web;
using System.Web.Http;

namespace Tenda.EmpresaVenda.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutofacConfig.ConfigureContainer();

            QuartzConfig.Config();
        }
    }
}
