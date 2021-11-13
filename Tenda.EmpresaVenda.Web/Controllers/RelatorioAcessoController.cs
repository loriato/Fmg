using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("SEG14")]
    public class RelatorioAcessoController : BaseController
    {
        private RelatorioAcessoService _relatorioAcessoService { get; set; }

        [BaseAuthorize("SEG14", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }
        [BaseAuthorize("SEG14", "ExportarRelatorios")]
        public FileContentResult Exportar(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _relatorioAcessoService.ExportarRelatorios(modifiedRequest, filtro);

            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"Relatorio_de_Acessos{date}.zip");
        }
    }
}