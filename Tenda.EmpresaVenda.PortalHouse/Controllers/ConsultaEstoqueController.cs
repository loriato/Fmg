using Europa.Extensions;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.ConsultaEstoque;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class ConsultaEstoqueController : BaseController
    {
        public JsonResult Empreendimento(DataSourceRequest request, FiltroConsultaEstoqueDto filtro)
        {
            filtro.DataSourceRequest = request;
            var results = EmpresaVendaApi.EstoqueEmpreendimento(filtro);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unidade(DataSourceRequest request, FiltroConsultaEstoqueDto filtro)
        {
            filtro.DataSourceRequest = request;
            var results = EmpresaVendaApi.EstoqueUnidade(filtro);
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}