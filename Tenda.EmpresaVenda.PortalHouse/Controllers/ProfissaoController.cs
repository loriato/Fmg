using Europa.Extensions;
using System.Web.Mvc;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class ProfissaoController : BaseController
    {
        [HttpGet]
        public JsonResult ListarProfissoesAutoComplete(DataSourceRequest request)
        {
            var result = EmpresaVendaApi.ListarProfissaoAutocomplete(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}