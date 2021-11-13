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
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC07")]
    public class EfetivarSolicitacaoResgateController : BaseController
    {
        public PontuacaoFidelidadeEmpresaVendaService _pontuacaoFidelidadeEmpresaVendaService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("GEC07", "AprovarResgate")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AprovarResgate(PontuacaoFidelidadeDTO pontuacao)
        {
            var response = new JsonResponse();

            try
            {
                _pontuacaoFidelidadeEmpresaVendaService.AprovarResgate(pontuacao);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format(GlobalMessages.SolicitacaoLiberadaSucesso));
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("GEC07", "ReprovarResgate")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ReprovarResgate(PontuacaoFidelidadeDTO pontuacao)
        {
            var response = new JsonResponse();

            try
            {
                _pontuacaoFidelidadeEmpresaVendaService.ReprovarResgate(pontuacao);
                response.Sucesso = true;
                response.Mensagens.Add(string.Format(GlobalMessages.SolicitacaoReprovada));
            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC07", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _pontuacaoFidelidadeEmpresaVendaService.ExportarEfetivarResgate(modifiedRequest, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.Extrato} - {DateTime.Now}.xlsx");
        }

        [BaseAuthorize("GEC07", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            byte[] file = _pontuacaoFidelidadeEmpresaVendaService.ExportarEfetivarResgate(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.Extrato} - {DateTime.Now}.xlsx");
        }

        [BaseAuthorize("GEC07", "ImportarTabelas")]
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
                $"RelatorioDeFaturamentos-Result.xlsx");
        }
    }
}