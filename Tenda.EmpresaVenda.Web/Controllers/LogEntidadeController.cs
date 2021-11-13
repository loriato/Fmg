using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using PortalPosVenda.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Shared.Commons;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class LogEntidadeController : BaseController
    {
        private LogEntidadeService _logEntidadeService { get; set; }

        public LogEntidadeController(ISession session, LogEntidadeService logEntidadeService) : base(session)
        {
            this._logEntidadeService = logEntidadeService;
        }

        [BaseAuthorize("SEG06", "Visualizar")]
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> entidades = Europa.Extensions.ReflectionHelper.SubclassOfBaseEntityExceptView().Select(x => new SelectListItem
            {
                Value = x.FullName,
                Text = x.FullName
            });

            return View(new LogEntidadeDTO
            {
                Entidades = entidades
            });
        }

        public JsonResult Listar(DataSourceRequest request, LogEntidadeDTO filtro)
        {
            var result = _logEntidadeService.Listar(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG06", "Exportar")]
        public ActionResult ExportarPagina(DataSourceRequest request, LogEntidadeDTO filtro)
        {
            byte[] file = _logEntidadeService.ExportarPagina(request, filtro);
            string nomeArquivo = GlobalMessages.LogEntidade;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("SEG06", "Exportar")]
        public ActionResult ExportarDatatable(DataSourceRequest request, LogEntidadeDTO filtro)
        {
            byte[] file = _logEntidadeService.ExportarDatatable(request, filtro);
            string nomeArquivo = GlobalMessages.LogEntidade;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

    }
}