using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD16")]
    public class AgenteVendaController : BaseController
    {
        [BaseAuthorize("CAD16", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD16", "Visualizar")]
        public ActionResult ListarDatatableLoja(DataSourceRequest request, FiltroLojaPortalDto filtro)
        {
            filtro.DataSourceRequest = request;
            var result = EmpresaVendaApi.ListarLojasPortal(filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD16", "Visualizar")]
        public ActionResult ListarDatatableUsuario(DataSourceRequest request, FiltroAgenteVendaDto filtro)
        {
            filtro.DataSourceRequest = request;
            var result = EmpresaVendaApi.ListarDatatableUsuario(filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("CAD16", "Incluir")]
        public ActionResult AtribuirUsuarioALoja(AgenteVendaDto model)
        {
            var result = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.AtribuirUsuarioLoja(model);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}