using Europa.Rest;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class JuridicoController : BaseController
    {
        private ArquivoService _arquivoService { get; set; }


        public ActionResult Index()
        {
            return View();
        }

        public JsonResult UploadDocumentoJuridico(HttpPostedFileBase file)
        {
            var fileDto = new FileDto();
            fileDto.FromHttpFile(file);

            var response = EmpresaVendaApi.UploadDocumentoJuridico(fileDto);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
