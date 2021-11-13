using Europa.Extensions;
using System.Web.Mvc;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class LojaPortalController : BaseController
    {
        [HttpGet]
        public JsonResult ListarLojas(DataSourceRequest request)
        {
            var result = EmpresaVendaApi.ListarLojasAutocomplete(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}