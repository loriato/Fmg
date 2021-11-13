using Europa.Commons;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("REL02")]
    public class FilaUnificadaController : BaseController
    {        
        public ViewFilaUnificadaRepository _viewFilaUnificadaRepository { get; set; }
        public UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }
        public GrupoCCAEmpresaVendaRepository _grupoCCAEmpresaVendaRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("REL02", "VisualizarFilaUnificada")]
        public JsonResult ListarDatatable(DataSourceRequest request,FiltroFilaUnificadaDTO filtro)
        {
            FiltroAuxiliar(ref filtro);

            var result = _viewFilaUnificadaRepository.Listar(request, filtro);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void FiltroAuxiliar(ref FiltroFilaUnificadaDTO filtro)
        {
            var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;
            var idsGrupos = _usuarioGrupoCCARepository.ListarIdGruposPorUsuario(idUsuario);
            var idsEvs = _grupoCCAEmpresaVendaRepository.ListarIdsEvsPorGrupo(idsGrupos);
            filtro.IdsEvs = idsEvs;

            var filas = filtro.Filas;
            if (filas.HasValue())
            {
                filtro.IdsNodes = new List<long>();

                var nodes = ProjectProperties.FilasSUAT
                    .Where(x => filas.Contains(x.IdFila)).Select(x=>x.Nodes);

                foreach(var n in nodes)
                {
                    filtro.IdsNodes.AddRange(n);
                }
            }            
        }

        [BaseAuthorize("REL02", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroFilaUnificadaDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            FiltroAuxiliar(ref filtro);

            byte[] file = _viewFilaUnificadaRepository.Exportar(modifiedRequest, filtro);
            string nomeArquivo = "FilaUnificadaSuatXEvs";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("REL02", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroFilaUnificadaDTO filtro)
        {
            FiltroAuxiliar(ref filtro);

            byte[] file = _viewFilaUnificadaRepository.Exportar(request, filtro);
            string nomeArquivo = "FilaUnificadaSuatXEvs";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("REL02", "ExportarTodos")]
        public FileContentResult ExportarTodosOperador(DataSourceRequest request, FiltroFilaUnificadaDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            FiltroAuxiliar(ref filtro);

            byte[] file = _viewFilaUnificadaRepository.ExportarOperador(modifiedRequest, filtro);
            string nomeArquivo = "FilaUnificadaSuatXEvs";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("REL02", "ExportarPagina")]
        public FileContentResult ExportarPaginaOperador(DataSourceRequest request, FiltroFilaUnificadaDTO filtro)
        {
            FiltroAuxiliar(ref filtro);

            byte[] file = _viewFilaUnificadaRepository.ExportarOperador(request, filtro);
            string nomeArquivo = "FilaUnificadaSuatXEvs";
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    }
}