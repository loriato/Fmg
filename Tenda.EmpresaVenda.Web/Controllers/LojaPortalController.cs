using Europa.Extensions;
using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.EmpresaVenda.ApiService.Models.Loja;
using Tenda.EmpresaVenda.Web.Rest;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD15")]
    public class LojaPortalController : BaseController
    {

        [BaseAuthorize("CAD15", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("CAD15", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroLojaPortalDto filtro)
        {
            filtro.DataSourceRequest = request;
            var result = EmpresaVendaApi.ListarLojasPortal(filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD15", "Visualizar")]
        public JsonResult Buscar(long id)
        {
            var loja = EmpresaVendaApi.BuscarLojaPortal(id);

            var result = new
            {
                htmlEmpresaVenda = RenderRazorViewToString("_FormularioLojaPortal", loja, false),
                htmlEnderecoEmpresaVenda = RenderRazorViewToString("_FormularioEndereco", loja, false)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("CAD15", "Incluir")]
        public JsonResult Incluir(LojaPortalDto model)
        {
            return Salvar(model);
        }

        [HttpPost]
        [BaseAuthorize("CAD15", "Editar")]
        public JsonResult Alterar(LojaPortalDto model)
        {
            return Salvar(model);
        }

        private JsonResult Salvar(LojaPortalDto model)
        {
            var result = new JsonResponse();
            try
            {
                var response = model.Id > 0
                    ? EmpresaVendaApi.EditarLojaPortal(model)
                    : EmpresaVendaApi.IncluirLojaPortal(model);
                result.FromBaseResponse(response);
                TempData["SucessMessage"] = response.Messages.FirstOrDefault();
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("CAD15", "Excluir")]
        public ActionResult Excluir(long id)
        {
            var json = new JsonResponse();
            try
            {
                var response = EmpresaVendaApi.ExcluirLojaPortal(id);
                json.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                json.FromBaseResponse(e.GetResponse());
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD15", "Suspender")]
        public JsonResult Suspender(long[] ids)
        {
            var result = new JsonResponse();

            try
            {
                var response = EmpresaVendaApi.SuspenderLoja(ids);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD15", "Cancelar")]
        public JsonResult Cancelar(long[] ids)
        {
            var result = new JsonResponse();

            try
            {
                var response = EmpresaVendaApi.CancelarLoja(ids);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD15", "Reativar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Reativar(long[] ids)
        {
            var result = new JsonResponse();

            try
            {
                var response = EmpresaVendaApi.ReativarLoja(ids);
                result.FromBaseResponse(response);
            }
            catch (ApiException e)
            {
                result.FromBaseResponse(e.GetResponse());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD15", "ExportarPagina")]
        public ActionResult ExportarPagina(DataSourceRequest request, FiltroLojaPortalDto filtro)
        {
            filtro.DataSourceRequest = request;
            var file = EmpresaVendaApi.ExportarPaginaLojasPortal(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }

        [BaseAuthorize("CAD15", "ExportarTodos")]
        public ActionResult ExportarTodos(DataSourceRequest request, FiltroLojaPortalDto filtro)
        {
            filtro.DataSourceRequest = request;
            var file = EmpresaVendaApi.ExportarTodosLojasPortal(filtro);
            return File(file.Bytes, file.ContentType, $"{file.FileName}.{file.Extension}");
        }

    }
}