using Europa.Extensions;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.PortalHouse.Controllers;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;
using Tenda.EmpresaVenda.PortalHouse.Rest;
using Tenda.EmpresaVenda.PortalHouse.Security;

namespace Tenda.EmpresaVenda.PortalHouse.Controllers
{
    public class ConsultaPrePropostaController : BaseController
    {
        public ActionResult Index(FiltroConsultaPrePropostaDto filtro)
        {
            return View();
        }

        [BaseAuthorize("EVS11", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, FiltroConsultaPrePropostaDto filtro)
        {
            filtro.CodigoSistema = ApplicationInfo.CodigoSistema;
            var results = EmpresaVendaApi.ListarPrepropostas(request,filtro);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS11", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroConsultaPrePropostaDto filtro)
        {
            filtro.DataSourceRequest = request;
            FileDto file = EmpresaVendaApi.ExportarTodosConsultarPreProposta(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }
        
        [BaseAuthorize("EVS11", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroConsultaPrePropostaDto filtro)
        {
            filtro.DataSourceRequest = request;
            FileDto file = EmpresaVendaApi.ExportarPaginaConsultarPreProposta(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }
    }
}