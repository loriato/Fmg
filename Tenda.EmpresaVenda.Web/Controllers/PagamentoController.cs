using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Imports;
using Tenda.EmpresaVenda.Domain.Integration.Zeus;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG05")]
    public class PagamentoController : BaseController
    {
        public PagamentoService _pagamentoService { get; set; }
        public ViewUsuarioEmpresaVendaRepository _viewUsuarioEmpresaVendaRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public ViewPagamentoRepository _viewPagamentoRepository { get; set; }
        private DateTime DataVendaGerada = new DateTime(2020, 6, 1);
        private RequisicaoCompraService _requisicaoCompraService { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private RequisicaoCompraSapService _requisicaoCompraSapService { get; set; }
        private ViewRequisicaoCompraSapRepository _viewRequisicaoCompraSapRepository { get; set; }
        private RequisicaoCompraSapRepository _requisicaoCompraSapRepository { get; set; }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "VisualizarPagamento")]
        public JsonResult ListarPagamentos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var json = new JsonResponse();

            try
            {

                var pagamentos = _pagamentoService.ListarPagamentos(request, filtro);

                var html = new StringBuilder();

                foreach (var pagamento in pagamentos)
                {
                    html.Append(RenderRazorViewToString("~/Views/Pagamento/_Datatable.cshtml", pagamento, false));
                }

                json.Sucesso = true;
                json.Objeto = html.ToString();

            }
            catch (BusinessRuleException bre)
            {
                json.Mensagens.AddRange(bre.Errors);
            }

            var result = Json(json, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = 500000000;
            return result;
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "IncluirPagamento")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarPagamento(Pagamento pagamento)
        {
            var result = new JsonResponse();
            try
            {
                _pagamentoService.SalvarPagamento(pagamento);
                result.Sucesso = true;
                result.Mensagens.Add(string.Format(GlobalMessages.SalvoSucesso));
            }
            catch (BusinessRuleException bre)
            {
                result.Mensagens.AddRange(bre.Errors);
                result.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "RemoverPagamento")]
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
        public JsonResult ValidarFiltro(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var response = new JsonResponse();
            var dataVenda = ProjectProperties.DataBuscaVendaGerada.HasValue() ? ProjectProperties.DataBuscaVendaGerada : DataVendaGerada;
            filtro.DataVendaDe = dataVenda;
            try
            {
                var result = _pagamentoService.ListarPagamentos(request, filtro);

                response.Sucesso = true;

            }
            catch (BusinessRuleException bre)
            {
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            byte[] file = _pagamentoService.ExportarPagamentos(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Relatório de pagamentos - {filtro.PeriodoDe.ToDate()} - {filtro.PeriodoAte.ToDate()}.xlsx");
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            request.pageSize = 0;
            request.start = 0;
            byte[] file = _pagamentoService.ExportarPagamentos(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Relatório de pagamentos - {filtro.PeriodoDe.ToDate()} - {filtro.PeriodoAte.ToDate()}.xlsx");
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "ExportarTodasEvs")]
        public FileContentResult ExportarTodasEvs(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var idUsuario = SessionAttributes.Current().UsuarioPortal.Id;
            var lista = _viewUsuarioEmpresaVendaRepository.BuscarEvsPorUsuario(idUsuario)
                .Select(x => x.IdEmpresaVenda).ToList();
            filtro.IdsEmpresaVenda = lista;
            byte[] file = _pagamentoService.ExportarPagamentos(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"Relatório de pagamentos - {filtro.PeriodoDe.ToDate()} - {filtro.PeriodoAte.ToDate()}.xlsx");
        }

        #region novo
        [HttpPost]
        public JsonResult DesenharTabelas(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var dados = _viewPagamentoRepository.ListarPagamentos(request, filtro).records.ToList();

            var evs = new Dictionary<long, string>();

            foreach (var res in dados)
            {
                if (!evs.ContainsKey(res.IdEmpresaVenda))
                {
                    evs.Add(res.IdEmpresaVenda, res.NomeEmpresaVenda);
                }
            }

            var html = new StringBuilder();

            foreach (var ev in evs)
            {
                var propostas = dados.Where(x => x.IdEmpresaVenda == ev.Key);

                var pagamento = new PagamentoDTO
                {
                    IdEmpresaVenda = ev.Key,
                    NomeEmpresaVenda = ev.Value,
                    TotalAPagar = propostas.Sum(x => x.ValorAPagar),
                    TotalPago = propostas.Where(x => x.Pago == true).Sum(x => x.ValorAPagar)
                };

                html.Append(RenderRazorViewToString("~/Views/Pagamento/_Datatable.cshtml", pagamento, false));
            }

            var result = new JsonResponse
            {
                Sucesso = true,
                Objeto = html.ToString()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListarTabela(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var result = _viewPagamentoRepository.ListarPagamentos(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [BaseAuthorize("PAG05", "ImportarTabelas")]
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
                var importService = new PagamentoImportService();

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
                $"RelatorioDePagamentos-Result.xlsx");
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "GerarRequisicaoCompra")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult GerarRequisicaoCompra(List<ViewPagamento> pagamentos)
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
                    var requisicao = _pagamentoService.GerarRequisicaoCompra(pagamentos, dadosAuditoria,processSession,diretorio,ref importTask);
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
            catch(Exception ex)
            {
                response.Sucesso = false;
                response.Mensagens.Add(ex.Message);

            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GerarRequisicaoCompraModal(ViewPagamento model)
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
        [BaseAuthorize("PAG05", "GerarRequisicaoCompra")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult GerarRC(List<RequisicaoCompraDTO> requisicaoCompra)
        {
            var bre = new BusinessRuleException();
            var json = new JsonResponse();

            var dadosAuditoria = new DadosAuditoriaDTO
            {
                Usuario = SessionAttributes.Current().UsuarioPortal.Login,
                Sistema = "EmpresaVenda",
                DataTransacao = DateTime.Now
            };
            json.Sucesso = true;
            foreach (var rc in requisicaoCompra)
            {
                rc.ContabilizacaoRequisicao.NumeroItem = rc.ItemRequisicao.NumeroItem;
                rc.ContabilizacaoRequisicao.Quantidade = rc.ItemRequisicao.Quantidade;

                var count = requisicaoCompra.AsQueryable().Where(x => x.Pagamento.IdPagamento == rc.Pagamento.IdPagamento)
                    .Where(x => x.Pagamento.TipoPagamento == rc.Pagamento.TipoPagamento)
                    .Where(x => x.Pagamento.IdProposta == rc.Pagamento.IdProposta)
                    .Count();

                if (count >= 2)
                {
                    json.Mensagens.Add(string.Format("Existe duas ou mais proposta {0} iguais nessa lista para geração de RC ", rc.Pagamento.CodigoProposta));
                    continue;
                }

                var result = ZeusService.GerarRC(rc.ItemRequisicao, rc.ContabilizacaoRequisicao, dadosAuditoria);

                foreach (var item in result)
                {

                    var requisicaoCompraSap = new RequisicaoCompraSap();
                    requisicaoCompraSap.Numero = item.numero;
                    requisicaoCompraSap.Status = item.status;
                    requisicaoCompraSap.Proposta = new PropostaSuat { Id = rc.Pagamento.IdProposta };
                    requisicaoCompraSap.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = rc.Pagamento.IdEmpresaVenda };
                    requisicaoCompraSap.TipoPagamento = rc.Pagamento.TipoPagamento;
                    requisicaoCompraSap.Texto = item.texto;
                    requisicaoCompraSap.NumeroGerado = item.status.IsEmpty();

                    _requisicaoCompraSapService.Salvar(requisicaoCompraSap);

                    if (requisicaoCompraSap.NumeroGerado)
                    {

                        _pagamentoService.SalvarRCPagamento(rc.Pagamento, item.numero);
                        json.Mensagens.Add(string.Format("Requisição compra número {0} gerada para a proposta {1}", item.numero, rc.Pagamento.CodigoProposta));
                    }
                    else
                    {
                        json.Sucesso = false;
                        json.Mensagens.Add(string.Format("{0}. referente a proposta {1}", item.status, rc.Pagamento.CodigoProposta));
                    }
                }
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG05", "VisualizarPagamento")]
        public ActionResult ListarRequisicaoCompraSap(DataSourceRequest request, FiltroRequisicaoCompraSapDTO filtro)
        {
            var results = _viewRequisicaoCompraSapRepository.Listar(filtro);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }
    }
}