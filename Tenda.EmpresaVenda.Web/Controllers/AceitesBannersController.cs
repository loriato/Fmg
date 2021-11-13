using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC16")]
    public class AceitesBannersController : BaseController
    {
        private AceitesBannersRepository _aceitesBannersRepository { get; set; }
        private AceiteBannerService _aceitesBannersService { get; set; }

        [BaseAuthorize("GEC16", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar(DataSourceRequest request, long? IdBanner)
        {
            var result = _aceitesBannersRepository.Listar(request,IdBanner);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC16", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, long? IdBanner)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            byte[] file = _aceitesBannersService.Exportar(modifiedRequest, IdBanner);
            string nomeArquivo = GlobalMessages.AceitesBanners;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        [BaseAuthorize("GEC16", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, long? IdBanner)
        {
            byte[] file = _aceitesBannersService.Exportar(request, IdBanner);
            string nomeArquivo = GlobalMessages.AceitesBanners;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    }
}