using System.Web.Mvc;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}