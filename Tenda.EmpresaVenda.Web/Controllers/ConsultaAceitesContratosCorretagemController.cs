using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Web.Security;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Europa.Extensions;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Europa.Commons;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC17")]
    public class ConsultaAceitesContratosCorretagemController : BaseController
    {
        // GET: ConsultaAceitesContratos

        public ConsultaAceiteContratoCorretagemService _consultaAceitesContratosCorretagemService { get; set; }

        public ArquivoRepository _arquivoRepository { get; set; }
        

        public ActionResult Index()
        {
            return View();
        }

       [HttpPost]
       [BaseAuthorize("GEC17", "Listar")]
       public JsonResult Listar(DataSourceRequest request, FiltroConsultaAceitesContratosCorretagemDTO filtro)
        {
            
            var result = _consultaAceitesContratosCorretagemService.Listar(request, filtro);

            var response = Json(result, JsonRequestBehavior.AllowGet);

            response.MaxJsonLength = 24000000;

            return response;

        }


        [HttpPost]
        [BaseAuthorize("GEC17", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroConsultaAceitesContratosCorretagemDTO filtro)
        {

            byte[] file = _consultaAceitesContratosCorretagemService.ExportarConsulta(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Consulta de Aceite de Contratos - {DateTime.Now}.xlsx");
        }


        [HttpPost]
        [BaseAuthorize("GEC17", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroConsultaAceitesContratosCorretagemDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            byte[] file = _consultaAceitesContratosCorretagemService.ExportarConsulta(request, filtro);
            string nomeArquivo = "Consulta de Aceite de Contratos";
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("GEC17", "BaixarArquivo")]
        public FileContentResult BaixarArquivo(long idArquivo)
        {
            var file = _arquivoRepository.FindById(idArquivo);
            return File(file.Content, file.ContentType, file.Nome);
        }

    }
}
