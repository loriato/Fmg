using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;


namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC10")]
    public class CargaValorNominalReferenciaController : BaseController
    {
        private ViewValorNominalEmpreendimentoRepository _viewValorNominalEmpreendimentoRepository { get; set; }
        private ValorNominalService _valorNominalService { get; set; }

        [BaseAuthorize("GEC10", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC10", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            var result = _viewValorNominalEmpreendimentoRepository.Listar(filtro);
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC10", "ImportarTabelas")]
        [HttpPost]
        public JsonResult UploadImportFile(HttpPostedFileBase file)
        {
            var bre = new BusinessRuleException();
            var json = new JsonResponse();
            try
            {

                bre.ThrowIfHasError();

                ImportTaskDTO importTask = new ImportTaskDTO();
                Session.Add(importTask.TaskId, importTask);
                var importService = new ValorNominalEmpreendimentoImportService();

                string diretorio = ProjectProperties.DiretorioArquivosTemporarios;
                importTask.FileName = file.FileName;
                importTask.FileName = file.FileName;
                importTask.OriginalFilePath = string.Format("{0}{1}{2}-source-{3}", diretorio, Path.DirectorySeparatorChar,
                    importTask.TaskId, file.FileName);

                using (FileStream stream =
                    new FileStream(importTask.OriginalFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    // Salvando arquivo origem em disco.
                    file.InputStream.CopyTo(stream);
                    file.InputStream.Position = 0;
                }

                ISession processSession = NHibernateSession.NestedScopeSession();

                Thread thread = new Thread(delegate ()
                {
                    importService.Process(processSession, diretorio, ref importTask);
                });
                thread.Start();

                return Json(new { Task = importTask, Sucesso = true }, JsonRequestBehavior.AllowGet);
            }

            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
                return Json(json, JsonRequestBehavior.AllowGet);
            }

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
                $"RelatorioDeCargaValorNominal-Result.xlsx");
        }
        [BaseAuthorize("GEC10", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _valorNominalService.Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.ValorNominal;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        [BaseAuthorize("GEC10", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            byte[] file = _valorNominalService.Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.ValorNominal;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

    }
}