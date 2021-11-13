using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Imports;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class EmpreendimentoController : BaseController
    {
        private EmpreendimentoService _empreendimentoService { get; set; }
        private EnderecoEmpreendimentoService _enderecoEmpreendimentoService { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        private ViewEmpreendimentoEnderecoRepository _viewEmpreendimentoEnderecoRepository { get; set; }
        private QuartzConfigurationRepository _quartzConfigurationRepository { get; set; }
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }

        [BaseAuthorize("EVS06", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("EVS06", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, FiltroEmpreendimentoDTO filtro)
        {
            var results =
                _viewEmpreendimentoEnderecoRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        private JsonResult Salvar(EnderecoEmpreendimento enderecoEmpreendimento, Empreendimento empreendimento)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();

            try
            {
                empreendimento = _empreendimentoService.Salvar(empreendimento, validation);

                enderecoEmpreendimento.Empreendimento = empreendimento;
                _enderecoEmpreendimentoService.Salvar(enderecoEmpreendimento, validation);

                validation.ThrowIfHasError();

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, empreendimento.Nome));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("EVS06", "Incluir")]
        public JsonResult Incluir(EnderecoEmpreendimento enderecoEmpreendimento, Empreendimento empreendimento)
        {
            return Salvar(enderecoEmpreendimento, empreendimento);
        }

        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("EVS06", "Alterar")]
        public JsonResult Atualizar(EnderecoEmpreendimento enderecoEmpreendimento, Empreendimento empreendimento)
        {
            return Salvar(enderecoEmpreendimento, empreendimento);
        }

        [BaseAuthorize("EVS06", "Visualizar")]
        [HttpGet]
        public ActionResult BuscarEmpreendimento(long idEmpreendimento)
        {
            var jsonResponse = new JsonResponse();

            if (idEmpreendimento == 0)
            {
                var empreendimento = new Empreendimento
                {
                    Divisao = "T01",
                    IdSuat = 1,
                    DisponivelCatalogo = true
                };
                var enderecoEmpreendimento = new EnderecoEmpreendimento();

                var result = new
                {
                    htmlEmpreendimento = RenderRazorViewToString("_FormularioEmpreendimento", empreendimento, false),
                    htmlEnderecoEmpreendimento =
                        RenderRazorViewToString("_FormularioEndereco", enderecoEmpreendimento, false)
                };

                jsonResponse.Objeto = result;
                jsonResponse.Sucesso = true;
            }
            else
            {
                try
                {
                    var empreendimento = _empreendimentoRepository.FindById(idEmpreendimento);
                    var enderecoEmpreendimento =
                        _enderecoEmpreendimentoRepository.FindByEmpreendimento(idEmpreendimento);
                    
                    var breveLançamentoAssociado = _breveLancamentoRepository.BuscarBreveLancamentoAssociado(idEmpreendimento);

                    //empreendimento.DisponivelCatalogo = !breveLançamentoAssociado.IsEmpty() ? breveLançamentoAssociado.DisponivelCatalogo : false;


                    var result = new
                    {
                        htmlEmpreendimento =
                            RenderRazorViewToString("_FormularioEmpreendimento", empreendimento, false),
                        htmlEnderecoEmpreendimento =
                            RenderRazorViewToString("_FormularioEndereco", enderecoEmpreendimento, false)
                    };

                    jsonResponse.Objeto = result;
                    jsonResponse.Sucesso = true;
                }
                catch (BusinessRuleException bre)
                {
                    jsonResponse.Mensagens.AddRange(bre.Errors);
                    jsonResponse.Sucesso = false;
                }
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("EVS06", "Excluir")]
        public JsonResult Excluir(long idEmpreendimento)
        {
            var json = new JsonResponse();
            var entidade = _empreendimentoRepository.FindById(idEmpreendimento);

            try
            {
                _empreendimentoService.ExcluirPorId(idEmpreendimento);
                CurrentTransaction().Commit();
                json.Sucesso = true;
                json.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, entidade.Nome,
                    GlobalMessages.Excluido.ToLower()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    CurrentTransaction().Rollback();
                    json.Mensagens.Add(string.Format(GlobalMessages.RemovidoSemSucesso, entidade.ChaveCandidata()));
                    json.Sucesso = false;
                }
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarEmpreendimentosAutocomplete(DataSourceRequest request)
        {
            var result = _empreendimentoService.ListarEmpreendimentos(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS06", "ImportarTabelas")]
        [HttpPost]
        public JsonResult UploadImportFile(HttpPostedFileBase file)
        {
            ImportTaskDTO importTask = new ImportTaskDTO();
            Session.Add(importTask.TaskId, importTask);
            var importService = new EmpreendimentoImportService();

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
                $"CargaEmpreendimento-Result.xlsx");
        }

        [BaseAuthorize("EVS06", "ExportarTodos")]
        [HttpPost]
        public ActionResult ExportarTodos(DataSourceRequest request, FiltroEmpreendimentoDTO dto)
        {
            request.start = 0;
            request.pageSize = 0;

            var file = _empreendimentoService.Exportar(request, dto);
            return ExportarBase(file);
        }

        [BaseAuthorize("EVS06", "ExportarPagina")]
        [HttpPost]
        public ActionResult ExportarPagina(DataSourceRequest request, FiltroEmpreendimentoDTO dto)
        {
            var file = _empreendimentoService.Exportar(request, dto);
            return ExportarBase(file);
        }

        private ActionResult ExportarBase(byte[] file)
        {
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx,
                $"TabelaEmpreendimento_{date}.xlsx");
        }

        [HttpPost]
        public ActionResult IntegrarEmpreendimento()
        {

            JsonResponse res = new JsonResponse();

            try
            {
                var quartz = _quartzConfigurationRepository.Queryable().Where(x => x.CaminhoCompleto == "Tenda.EmpresaVenda.Web.Jobs.IntegrarEmpreendimentoSuatJob").FirstOrDefault();
                ParametroRoboController parametroRobo = new ParametroRoboController();
                parametroRobo.Executar(quartz);

                res.Sucesso = true;
                res.Mensagens.Add("Integração finalizada!");

            }
            catch (Exception e)
            {
                res.Sucesso = false;
                res.Mensagens.Add(string.Format("Erro ao integrar Empreendimentos: {0}", e.Message));
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListarTreeEmpreendimentos(long? idPontuacaoFidelidadeReferencia, String regional)
        {
            if (idPontuacaoFidelidadeReferencia.IsEmpty() && regional.IsEmpty())
            {
                return Json(new List<TreeViewModel>(), JsonRequestBehavior.AllowGet);
            }

            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .OrderByDescending(x => x.Nome)
                .ThenBy(x => x.Nome)
                .ToList();

            var empreendimentosComPontuacao = new List<long>();

            if (idPontuacaoFidelidadeReferencia.HasValue())
            {
                empreendimentosComPontuacao = _itemPontuacaoFidelidadeRepository.Queryable()
                    .Where(x => x.PontuacaoFidelidade.Id == idPontuacaoFidelidadeReferencia)
                    .Select(x => x.Empreendimento.Id)
                    .ToList();                
            }

            var results = empreendimentos.Select(x => new TreeViewModel
            {
                id = x.Id,
                text = x.Nome,
                selectable = true,
                state = new TreeViewState
                {
                    @checked = empreendimentosComPontuacao.Contains(x.Id),
                    disabled = false,
                    expanded = false,
                    selected = false
                }
            }).OrderBy(x => x.text);
            return Json(results.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}