using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC15")]
    public class PropostaFaturadaController : BaseController
    {
        private ViewRelatorioComissaoRepository _viewRelatorioComissaoRepository { get; set; }
        private ViewPropostaFaturadaRepository _viewPropostaFaturadaRepository { get; set; }
        private PropostaSuatService _propostaSuatService { get; set; }

        [BaseAuthorize("GEC15", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC15", "Visualizar")]
        public JsonResult Listar(DataSourceRequest request, FiltroPropostaDTO filtro)
        {
            filtro.Faturado = 1;
            var response = _viewPropostaFaturadaRepository.Listar(request,filtro);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region Import Excel
        [BaseAuthorize("GEC15", "ImportarExcel")]
        [HttpPost]
        public JsonResult ImportExcel(HttpPostedFileBase file)
        {
            var bre = new BusinessRuleException();
            var json = new JsonResponse();
            try
            {

                ImportTaskDTO importTask = new ImportTaskDTO();
                Session.Add(importTask.TaskId, importTask);
                var importService = new PontuacaoFidelidadeImportService();

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
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC15", "ImportarExcel")]
        public JsonResult VerificarStatusImportacao(string taskId)
        {
            var task = HttpContext.Session[taskId];

            if (task != null)
            {
                return Json(new { Task = (ImportTaskDTO)task }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC15", "DownloadArquivo")]
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
                $"RelatorioDeFaturamentos-Result.xlsx");
        }
        #endregion

        #region Exportar
        [BaseAuthorize("GEC15", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroPropostaDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _propostaSuatService.ExportarPropostaFaturada(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.PropostasFaturadas;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("GEC15", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroPropostaDTO filtro)
        {
            byte[] file = _propostaSuatService.ExportarPropostaFaturada(request, filtro);
            string nomeArquivo = GlobalMessages.PropostasFaturadas;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        
        #endregion
    }
}