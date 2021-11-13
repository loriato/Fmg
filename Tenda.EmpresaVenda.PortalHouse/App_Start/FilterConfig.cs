using System.Web.Mvc;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());
        }
    }
}
