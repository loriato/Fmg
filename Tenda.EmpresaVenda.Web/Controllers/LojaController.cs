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
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Jobs;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class LojaController : BaseController
    {
        public LojaRepository _lojaRepository { get; set; }
        public LojaService _lojaService { get; set; }
        public QuartzConfigurationRepository _quartzConfigurationRepository { get; set; }

        [BaseAuthorize("EVS13", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS13", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, LojaDto filtro)
        {
            var results = _lojaRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarAutoCompleteDisponiveis(DataSourceRequest request)
        {
            LojaDto dto = new LojaDto();

            // Autocomplete 
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    dto.Nome = queryTerm;
                }
            }

            var result = _lojaService.ListarDisponiveis(dto).Select(reg => new Loja
            {
                Id = reg.Id,
                Nome = reg.Nome,
            });
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }


        [BaseAuthorize("EVS13", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, LojaDto dto)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, dto);
            string nomeArquivo = GlobalMessages.Loja;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        [BaseAuthorize("EVS13", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, LojaDto filtro)
        {
            byte[] file = Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.Loja;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, LojaDto filtro)
        {
            var results = _lojaRepository.Listar(filtro).ToDataRequest(request);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string formatDate = "dd/MM/yyyy";
            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Nome).Width(25)
                    .CreateCellValue(model.NomeFantasia).Width(20)
                    .CreateCellValue(model.SapId).Width(40)
                    .CreateCellValue(model.Regional.Nome).Width(10)
                    .CreateCellValue(model.DataIntegracao.ToString()).Width(10)
                    .CreateCellValue(model.Situacao.AsString()).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Nome,
                GlobalMessages.NomeFantasia,
                GlobalMessages.Sapid,
                GlobalMessages.Regional,
                GlobalMessages.DataIntegracao,
                GlobalMessages.Situacao
            };
            return header.ToArray();
        }

        [BaseAuthorize("EVS13", "ImportarTabelas")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UploadFile(HttpPostedFileBase file)
        {
            ImportTaskDTO importTask = new ImportTaskDTO();
            var response = new JsonResponse();

            if (file.IsNull())
            {
                response.Sucesso = false;
                response.Mensagens.Add(GlobalMessages.ArquivoAnexoNaoSelecionado);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else if (file.ContentLength > 20000000)
            {
                response.Sucesso = false;
                response.Mensagens.Add(string.Format(GlobalMessages.TamanhoArquivoExcedido, "20MB"));
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            Session.Add(importTask.TaskId, importTask);
            LojaImportService importService = new LojaImportService();

            string diretorio = ProjectProperties.DiretorioArquivosTemporarios;
            importTask.FileName = file.FileName;
            importTask.OriginalFilePath = string.Format("{0}{1}{2}-source-{3}", diretorio, Path.DirectorySeparatorChar, importTask.TaskId, file.FileName);

            using (FileStream stream = new FileStream(importTask.OriginalFilePath, FileMode.Create, FileAccess.ReadWrite))
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

            return Json(new { Sucesso = true, Task = importTask }, JsonRequestBehavior.AllowGet);
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

            byte[] arquivo = System.IO.File.ReadAllBytes(task.TargetFilePath);

            return File(arquivo, MimeMappingWrapper.Xlsx, $"Importacao-Loja-Result.xlsx");
        }

        public ActionResult IntegrarLoja()
        {
            JsonResponse res = new JsonResponse();

            try
            {
                var quartz = _quartzConfigurationRepository.Queryable().Where(x => x.CaminhoCompleto == "Tenda.EmpresaVenda.Web.Jobs.IntegrarLojaSuat").FirstOrDefault();
                ParametroRoboController parametroRobo = new ParametroRoboController();
                parametroRobo.Executar(quartz);

                res.Mensagens.Add("Integração finalizada!");

            }
            catch (Exception e)
            {
                res.Sucesso = false;
                res.Mensagens.Add(string.Format("Erro ao integrar Lojas: {0}", e.Message));
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }
    }
}