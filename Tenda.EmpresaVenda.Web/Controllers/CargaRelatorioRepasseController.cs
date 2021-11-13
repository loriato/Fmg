using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Imports;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Web.Security;
using Tenda.EmpresaVenda.Domain.Data;
using Europa.Commons;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Europa.Web;
using Europa.Resources;
using Europa.Extensions;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Web.App_Start;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class CargaRelatorioRepasseController : BaseController
    {
        public ImportacaoJunixService _importacaoJunixService { get; set; }
        public ArquivoService _arquivoService { get; set; }
        public ViewCargarelatorioRepasseJunixRepository _viewCargarelatorioRepasseJunixRepository { get; set; }
        public QuartzConfigurationRepository _quartzConfigurationRepository { get; set; }
        public ArquivoRepository _arquivoRepository { get; set; }
        public ImportacaoJunixRepository _importacaoJunixRepositoy { get; set; }

        [BaseAuthorize("PAG02", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("PAG02", "ImportarTabelas")]        
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadImportFile(HttpPostedFileBase file)
        {
            BusinessRuleException bre = new BusinessRuleException();
            var result = new JsonResponse();
            try
            {
                var arquivo = _arquivoService.CreateFile(file);

                long rowCount = _arquivoService.TotalLinhas(arquivo);

                var importacaoJunix = new ImportacaoJunix()
                {
                    Situacao = SituacaoArquivo.AguardandoProcessamento,
                    Origem = TipoOrigem.Manual,
                    Arquivo = arquivo,
                    TotalRegistros = rowCount
                };

                _importacaoJunixService.Salvar(importacaoJunix);

                result.Sucesso = true;
                result.Mensagens.Add(string.Format(GlobalMessages.ArquivoEnviadoSucesso, file.FileName));

            }catch(BusinessRuleException ex)
            {
                result.Mensagens.AddRange(ex.Errors);
                result.Campos.AddRange(ex.ErrorsFields);
                result.Sucesso = false;                
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult VerificarStatusImportacao(string taskId)
        {
            var task = HttpContext.Session[taskId];

            if (task != null)
            {
                return Json(new { Task = (ImportTaskDTO)task }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadArquivo(string taskId)
        {
            var objTask = HttpContext.Session[taskId];
            if (objTask == null)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            var task = (ImportTaskDTO)objTask;

            var arquivo = System.IO.File.ReadAllBytes(task.TargetFilePath);

            return File(arquivo, MimeMappingWrapper.Xlsx,
                $"Assinatura-Registro.xlsx");
        }

        [HttpPost]
        public ActionResult ListarDatatable(DataSourceRequest request, CargaRelatorioRepasseDTO filtro
            )
        {
            var result = _viewCargarelatorioRepasseJunixRepository.ListarDatatable(request,filtro);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DownloadArquivoImportacao(long idArquivo)
        {
            var arquivo = _arquivoRepository.FindById(idArquivo);
            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        [HttpPost]
        [BaseAuthorize("PAG02", "Executar")]
        public ActionResult IntegrarRepasseJunix()
        {
            var response = new JsonResponse();
            var param = _quartzConfigurationRepository.Queryable().Where(x => x.CaminhoCompleto == "Tenda.EmpresaVenda.Web.Jobs.RepasseJunixJob").FirstOrDefault();
            try
            {
                QuartzConfig config = new QuartzConfig(CurrentSession());
                config.ExecutarJobAgora(param);
                response.Mensagens.Add(GlobalMessages.ExecucaoRoboIniciada);
                response.Sucesso = true;
            }
            catch (BusinessRuleException exc)
            {
                response.Sucesso = false;
                response.FromException(exc);
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [BaseAuthorize("PAG02", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, CargaRelatorioRepasseDTO dto)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, dto);
            string nomeArquivo = GlobalMessages.RelatorioDeVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("PAG02", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, CargaRelatorioRepasseDTO filtro)
        {
            byte[] file = Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.RelatorioDeVenda;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, CargaRelatorioRepasseDTO filtro)
        {
            var results = _viewCargarelatorioRepasseJunixRepository.ListarDatatable(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy HH:mm:ss";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.NomeArquivo).Width(50)
                    .CreateCellValue(model.NomeUsuario).Width(40)
                    .CreateCellValue(model.CriadoEm.HasValue() ? model.CriadoEm.ToString(formatDate) : empty).Width(25)
                    .CreateCellValue(model.Origem).Width(20)
                    .CreateCellValue(model.Situacao.ToString()).Width(25)
                    .CreateCellValue(model.DataInicio.HasValue ? model.DataInicio.Value.ToString(formatDate) : empty).Width(25)
                    .CreateCellValue(model.DataFim.HasValue ? model.DataFim.Value.ToString(formatDate) : empty).Width(25);
               
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.NomeArquivo,
                GlobalMessages.CriadoPor,
                GlobalMessages.CriadoEm,
                GlobalMessages.Origem,
                GlobalMessages.Situacao,
                GlobalMessages.DataInicio,
                GlobalMessages.DataFim
            };
            return header.ToArray();
        }

        [HttpGet]
        public bool BuscarImportacaoEmProcessamento()
        {

            return _importacaoJunixRepositoy.BuscarUmaImportacaoEmProcessamento();
        }
    }

}