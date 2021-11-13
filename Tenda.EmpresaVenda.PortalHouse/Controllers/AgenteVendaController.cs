using Europa.Extensions;
using System.Web.Mvc;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class AgenteVendaController : BaseController
    {
        [HttpGet]
        public JsonResult ListarAgentesVenda(DataSourceRequest request)
        {
            var result = EmpresaVendaApi.ListarAgentesVendaAutocomplete(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}