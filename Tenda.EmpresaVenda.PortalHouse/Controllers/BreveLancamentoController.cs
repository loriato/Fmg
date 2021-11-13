using Europa.Extensions;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class BreveLancamentoController : BaseController
    {
        [HttpGet]
        public JsonResult Listar(DataSourceRequest request)
        {
            var result = EmpresaVendaApi.ListarBreveLancamentoDaRegionalSemEmpreendimento(new FilterDto { DataSourceRequest = request });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarDaRegionalSemEmpreendimento(DataSourceRequest request)
        {
            var result = EmpresaVendaApi.ListarBreveLancamentoDaRegionalSemEmpreendimento(new FilterDto { DataSourceRequest = request});

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDaRegional(DataSourceRequest request)
        {
            var result = EmpresaVendaApi.ListarBreveLancamentoDaRegional(new FilterDto { DataSourceRequest = request });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTorre(DataSourceRequest request)
        {
            var results = EmpresaVendaApi.ListarTorres(new FilterDto { DataSourceRequest = request });
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}