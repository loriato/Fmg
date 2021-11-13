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
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{

    [BaseAuthorize("PAG13")]
    public class PagamentoUnificadoController : BaseController
    {
        private ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository { get; set; }
        private PagamentoService _pagamentoService { get; set; }
        private RequisicaoCompraService _requisicaoCompraService { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private ViewRequisicaoCompraSapRepository _viewRequisicaoCompraSapRepository { get; set; }

        private DateTime DataVendaGerada = ProjectProperties.DataBuscaVendaGerada.HasValue() ? ProjectProperties.DataBuscaVendaGerada : new DateTime(2020, 6, 1);


        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListarPagamentos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro = FiltroAuxiliar(filtro);

            var response = _viewRelatorioVendaUnificadoRepository.ListarRelatorioVendaUnificado(request, filtro);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG13", "GerarRequisicaoCompra")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult GerarRequisicaoCompraEmLote(List<ViewRelatorioVendaUnificado> pagamentos)
        {
            var response = new JsonResponse();

            try
            {
                ImportTaskDTO importTask = new ImportTaskDTO();
                Session.Add(importTask.TaskId, importTask);

                string diretorio = ProjectProperties.DiretorioArquivosTemporarios;

                var dadosAuditoria = new DadosAuditoriaDTO
                {
                    Usuario = SessionAttributes.Current().UsuarioPortal.Login,
                    Sistema = "EmpresaVenda",
                    DataTransacao = DateTime.Now
                };

                importTask.AppendLog(dadosAuditoria.ToString());

                ISession processSession = NHibernateSession.NestedScopeSession();
                Thread thread = new Thread(delegate ()
                {
                    var requisicao = _pagamentoService.GerarRequisicaoCompra(pagamentos, dadosAuditoria, processSession, diretorio, ref importTask);
                    //importService.Process(processSession, diretorio, ref importTask);
                });
                thread.Start();

                return Json(new { Task = importTask, Sucesso = true }, JsonRequestBehavior.AllowGet);

            }
            catch (BusinessRuleException bre)
            {
                /*Caso haja alguma exceço de regra de negócio, salva o que foi feito corretamente
                 * pois já gerou RC no SAP
                */
                _session.Transaction.Commit();
                response.Sucesso = false;
                response.Mensagens.AddRange(bre.Errors);
            }
            catch (Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerificarStatusRequisicaoCompra(string taskId)
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
                $"RelatorioDePagamentos-Result.xlsx");
        }

        public RelatorioComissaoDTO FiltroAuxiliar(RelatorioComissaoDTO filtro)
        {
            filtro.Faturado = 1;

            if (filtro.DataVendaDe.IsEmpty())
            {
                filtro.DataVendaDe = DataVendaGerada;
            }else if (filtro.DataVendaDe.Value.Date < DataVendaGerada.Date)
            {
                filtro.DataVendaDe = DataVendaGerada;
            }

            if (filtro.Estados.HasValue() && (filtro.Estados.Contains("") || filtro.Estados.Contains(null)))
            {
                filtro.Estados.Clear();
            }

            filtro.TipoEmpresaVenda = TipoEmpresaVenda.EmpresaVenda;

            return filtro;
        }

        public JsonResult VisualizarRequisicaoCompraModal(ViewRelatorioVendaUnificado model)
        {

            ItemRequisicaoDTO itemRequisicao = new ItemRequisicaoDTO();
            _requisicaoCompraService.GetParametros(itemRequisicao);


            itemRequisicao.TextoBreve = string.Format(itemRequisicao.TextoBreve, model.CodigoProposta);
            itemRequisicao.DataLiberacao = DateTime.Now;
            itemRequisicao.DataRemessaItem = DateTime.Now;
            itemRequisicao.DataSolicitacao = DateTime.Now;
            itemRequisicao.PrecoUnidade = 1;
            var empreendimento = _empreendimentoRepository.FindById(model.IdEmpreendimento);
            itemRequisicao.CentroDeCusto = empreendimento.Divisao;
            itemRequisicao.Preco = Convert.ToDouble(model.ValorAPagar);

            var fornecedorPretendido = _empresaVendaRepository.Queryable()
                .Where(reg => reg.Id == model.IdEmpresaVenda)
                .Select(reg => reg.CodigoFornecedor)
                .FirstOrDefault();
            itemRequisicao.FornecedorPretendido = fornecedorPretendido;

            ContabilizacaoRequisicaoDTO contabilizacaoRequisicao = new ContabilizacaoRequisicaoDTO();

            var codigo = "000";
            if (empreendimento.CodigoEmpresa.ToUpper() == "0070")
            {
                codigo = "290";
            }
            else if (empreendimento.CodigoEmpresa.ToUpper() != "0070")
            {
                codigo = "291";
            }
            var numeroOrdem = empreendimento.Divisao.Substring(0, empreendimento.Divisao.Length - 1) + "I" + codigo;
            contabilizacaoRequisicao.NumeroOrdem = numeroOrdem;
            contabilizacaoRequisicao.Divisao = empreendimento.Divisao;


            var result = new
            {
                htmlItemRequisicao = RenderRazorViewToString("~/Views/Pagamento/_ItemRequisicao.cshtml", itemRequisicao, false),
                htmlDesignacaoContaRequisicao = RenderRazorViewToString("~/Views/Pagamento/_DesignacaoContaRequisicao.cshtml", contabilizacaoRequisicao, false),
            };
            var json = new JsonResponse();
            json.Objeto = result;
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG13", "VisualizarPagamento")]
        public ActionResult ListarRequisicaoCompraSap(DataSourceRequest request, FiltroRequisicaoCompraSapDTO filtro)
        {
            var results = _viewRequisicaoCompraSapRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG13", "RemoverPagamento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult RemoverPagamento(long idPagamento)
        {
            var result = new JsonResponse();

            try
            {
                result.Sucesso = _pagamentoService.RemoverPagamento(idPagamento);
                if (result.Sucesso)
                {
                    result.Mensagens.Add(string.Format(GlobalMessages.RemovidoSucesso));
                }
                else
                {
                    result.Mensagens.Add(string.Format(GlobalMessages.PedidoRevercaoNaoEncontrado));
                }

            }
            catch (BusinessRuleException bre)
            {
                result.Mensagens.AddRange(bre.Errors);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG13", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro = FiltroAuxiliar(filtro);

            byte[] file = _pagamentoService.ExportarPagamentosUnificado(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Relatório de pagamentos - {filtro.PeriodoDe.ToDate()} - {filtro.PeriodoAte.ToDate()}.xlsx");
        }

        [HttpPost]
        [BaseAuthorize("PAG13", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            filtro = FiltroAuxiliar(filtro);

            request.pageSize = 0;
            request.start = 0;
            byte[] file = _pagamentoService.ExportarPagamentosUnificado(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Relatório de pagamentos - {filtro.PeriodoDe.ToDate()} - {filtro.PeriodoAte.ToDate()}.xlsx");
        }

        [BaseAuthorize("PAG13", "ImportarTabelas")]
        [HttpPost]
        public JsonResult UploadImportFile(HttpPostedFileBase file)
        {
            var json = new JsonResponse();
            try
            {
                ImportTaskDTO importTask = new ImportTaskDTO();
                Session.Add(importTask.TaskId, importTask);
                var importService = new PagamentoUnificadoImportService();

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
    }
}