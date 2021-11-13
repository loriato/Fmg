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
using Tenda.Domain.EmpresaVenda.Imports;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC04")]
    public class CargaLeadController : BaseController
    {
        private LeadRepository _leadRepository { get; set; }
        private OrigemLeadRepository _origemLeadRepository { get; set; }

        [BaseAuthorize("GEC04", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC04", "ImportarTabelas")]
        [HttpPost]
        public JsonResult UploadImportFile(HttpPostedFileBase file, ImportLeadDTO importLead)
        {
            var bre = new BusinessRuleException();
            var json = new JsonResponse();
            try
            {
                if (importLead.Pacote.IsEmpty())
                {
                    bre.Errors.Add(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Pacote));
                    bre.ErrorsFields.Add("Pacote");
                }
                else if (_leadRepository.CheckIfOtherPacoteInLead(importLead.Pacote))
                {
                    bre.Errors.Add(GlobalMessages.MsgErroPacoteExistente);
                    bre.ErrorsFields.Add("Pacote");
                }

                var origem = _origemLeadRepository.OrigemLeadValido(ProjectProperties.IdUsuarioSistema);

                if (origem.IsEmpty())
                {
                    bre.AddError(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.Usuario)).Complete();                    
                }

                bre.ThrowIfHasError();

                ImportTaskDTO importTask = new ImportTaskDTO();
                Session.Add(importTask.TaskId, importTask);
               
                var importService = new LeadImportService(origem.Id);

                string diretorio = ProjectProperties.DiretorioArquivosTemporarios;
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
                    importService.Process(processSession, diretorio, ref importTask, importLead);
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

        [BaseAuthorize("GEC04", "ImportarTabelas")]
        [HttpPost]
        public JsonResult UploadImportFileEmMassa(HttpPostedFileBase arquivo)
        {
            var origem = _origemLeadRepository.OrigemLeadValido(ProjectProperties.IdUsuarioSistema);

            ImportTaskDTO importTask = new ImportTaskDTO();
            Session.Add(importTask.TaskId, importTask);
            var importService = new LeadEmMassaImportService(origem.Id);

            string diretorio = ProjectProperties.DiretorioArquivosTemporarios;
            importTask.FileName = arquivo.FileName;
            importTask.OriginalFilePath = string.Format("{0}{1}{2}-source-{3}", diretorio, Path.DirectorySeparatorChar,
                importTask.TaskId, arquivo.FileName);

            using (FileStream stream =
                new FileStream(importTask.OriginalFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                // Salvando arquivo origem em disco.
                arquivo.InputStream.CopyTo(stream);
                arquivo.InputStream.Position = 0;
            }

            ISession processSession = NHibernateSession.NestedScopeSession();

            Thread thread = new Thread(delegate ()
            {
                importService.Process(processSession, diretorio, ref importTask);
            });
            thread.Start();

            return Json(new { Task = importTask }, JsonRequestBehavior.AllowGet);

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
                $"RelatorioDeAtendimentos-Result.xlsx");
        }


    }
}