using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using PortalPosVenda.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class LogAcaoController : BaseController
    {
        private LogAcaoService _logAcaoService { get; set; }
        private SistemaRepository _repositorioSistema { get; set; }

        public LogAcaoController(ISession session) : base(session)
        {

        }

        [BaseAuthorize("SEG05", "Visualizar")]
        public ActionResult Index()
        {

            IEnumerable<SelectListItem> sistemas = _repositorioSistema.Queryable().Select(x => new SelectListItem
            {
                Value = x.Nome,
                Text = x.Nome
            });

            return View(new LogAcaoDTO
            {
                Sistemas = sistemas
            });
        }

        public JsonResult Listar(DataSourceRequest request, LogAcaoDTO filtro)
        {
            var result = _logAcaoService.Listar(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarUnidadeFuncionalAutocomplete(DataSourceRequest request)
        {
            var result = _logAcaoService.ListarUnidadeFuncionalAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarFuncionalidadeAutocomplete(DataSourceRequest request)
        {
            var result = _logAcaoService.ListarFuncionalidadeAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [BaseAuthorize("SEG05", "Exportar")]
        public ActionResult ExportarPagina(DataSourceRequest request, LogAcaoDTO filtro)
        {
            byte[] file = _logAcaoService.ExportarPagina(request, filtro);
            string nomeArquivo = GlobalMessages.LogEntidade;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("SEG05", "Exportar")]
        public ActionResult ExportarDatatable(DataSourceRequest request, LogAcaoDTO filtro)
        {
            byte[] file = _logAcaoService.ExportarDatatable(request, filtro);
            string nomeArquivo = GlobalMessages.LogEntidade;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

    }
}