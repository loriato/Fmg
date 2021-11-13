using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.TiposDocumento;
using Tenda.EmpresaVenda.Web.Security;


namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("CAD18")]
    public class TiposDocumentoController : BaseController
    {
        // GET: TiposDocumento
        [BaseAuthorize("CAD18", "Visualizar")]
        public ActionResult Index()
        {

            return View();
        }

        [BaseAuthorize("CAD18", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroTiposDocumentoDTO dto)
        {
            dto.Request = request;
            var response = EmpresaVendaApi.ListarDatatableTiposDocumento(dto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD18", "Incluir")]
        public ActionResult Salvar(FiltroTiposDocumentoDTO documento)
        {
            documento.Obrigatorio = false;
            var response = EmpresaVendaApi.IncluirTipoDocumento(documento);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD18", "Alterar")]
        public ActionResult Alterar(FiltroTiposDocumentoDTO documento)
        {
            var response = EmpresaVendaApi.AlterarTipoDocumento(documento);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("CAD18", "Excluir")]
        public ActionResult Excluir(FiltroTiposDocumentoDTO documento)
        {
            var response = EmpresaVendaApi.ExcluirTipoDocumento(documento);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


    }
}