using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.ApiService.Models.CockpitMidas;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC21")]
    public class CockpitMidasController : BaseController
    {
        [BaseAuthorize("GEC21", "Visualizar")]
        public ActionResult Index()
        {

            return View();
        }

        [BaseAuthorize("GEC21", "Visualizar")]
        public ActionResult ListarOcorrencias(DataSourceRequest request, FiltroCockpitMidas filtro)
        {
            filtro.Request = request;
            var response = EmpresaVendaApi.ListarOcorrencias(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [BaseAuthorize("GEC21", "Visualizar")]
        public ActionResult ListarNotas(DataSourceRequest request, FiltroCockpitMidas filtro)
        {
            filtro.Request = request;
            var response = EmpresaVendaApi.ListarNotas(filtro);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC21", "ExportarTodos")]
        public FileContentResult ExportarTodosOcorrencias(DataSourceRequest request, FiltroCockpitMidas filtro)
        {
            filtro.Request = request;
            FileDto file = EmpresaVendaApi.ExportarTodosOcorrencias(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }

        [BaseAuthorize("GEC21", "ExportarPagina")]
        public FileContentResult ExportarPaginaOcorrencias(DataSourceRequest request, FiltroCockpitMidas filtro)
        {
            filtro.Request = request;
            FileDto file = EmpresaVendaApi.ExportarPaginaOcorrencias(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }

        [BaseAuthorize("GEC21", "ExportarTodos")]
        public FileContentResult ExportarTodosNotas(DataSourceRequest request, FiltroCockpitMidas filtro)
        {
            filtro.Request = request;
            FileDto file = EmpresaVendaApi.ExportarTodosNotas(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }

        [BaseAuthorize("GEC21", "ExportarPagina")]
        public FileContentResult ExportarPaginaNotas(DataSourceRequest request, FiltroCockpitMidas filtro)
        {
            filtro.Request = request;
            FileDto file = EmpresaVendaApi.ExportarPaginaNotas(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }

    }
}