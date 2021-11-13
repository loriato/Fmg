using Europa.Web;
using System.Net.Http;
using System.Web.Http;
using Tenda.EmpresaVenda.Api.Security;
using Tenda.EmpresaVenda.ApiService.Models.CockpitMidas;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Api.Controllers
{
    [RoutePrefix("api/cockpitmidas")]
    public class CockpitMidasController : BaseApiController
    {
        private CockpitMidasService _cockpitMidasService { get; set; }
        [HttpPost]
        [Route("listarocorrencias")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ListarOcorrencias(FiltroCockpitMidas filtro)
        {
            var dataSource = _cockpitMidasService.ListarOcorrencias(filtro);
            return Response(dataSource);
        }

        [HttpPost]
        [Route("listarnotas")]
        [AuthenticateUserByToken]
        public HttpResponseMessage ListarNotas(FiltroCockpitMidas filtro)
        {
            var dataSource = _cockpitMidasService.ListarNotas(filtro);
            return Response(dataSource);
        }

        [HttpPost]
        [Route("exportarTodosOcorrencias")]
        [AuthenticateUserByToken("GEC21", "ExportarTodos")]
        [IgnoreRequestResponseLog(IgnoreRequestResponseLogType.Response)]
        public HttpResponseMessage ExportarTodosOcorrencias(FiltroCockpitMidas filtro)
        {
            filtro.Request.start = 0;
            filtro.Request.pageSize = 0;
            var result = _cockpitMidasService.MontarExportarDtoOcorrencias(filtro);
            return Response(result);
        }

        [HttpPost]
        [Route("exportarPaginaOcorrencias")]
        [AuthenticateUserByToken("GEC21", "ExportarPagina")]
        [IgnoreRequestResponseLog(IgnoreRequestResponseLogType.Response)]
        public HttpResponseMessage ExportarPaginaOcorrencias(FiltroCockpitMidas filtro)
        {
            var result = _cockpitMidasService.MontarExportarDtoOcorrencias(filtro);
            return Response(result);
        }

        [HttpPost]
        [Route("exportarTodosNotas")]
        [AuthenticateUserByToken("GEC21", "ExportarTodos")]
        [IgnoreRequestResponseLog(IgnoreRequestResponseLogType.Response)]
        public HttpResponseMessage ExportarTodosNotas(FiltroCockpitMidas filtro)
        {
            filtro.Request.start = 0;
            filtro.Request.pageSize = 0;
            var result = _cockpitMidasService.MontarExportarDtoNotas(filtro);
            return Response(result);
        }

        [HttpPost]
        [Route("exportarPaginaNotas")]
        [AuthenticateUserByToken("GEC21", "ExportarPagina")]
        [IgnoreRequestResponseLog(IgnoreRequestResponseLogType.Response)]
        public HttpResponseMessage ExportarPaginaNotas(FiltroCockpitMidas filtro)
        {
            var result = _cockpitMidasService.MontarExportarDtoNotas(filtro);
            return Response(result);
        }
    }
}