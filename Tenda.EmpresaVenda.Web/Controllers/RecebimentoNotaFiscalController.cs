using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG07")]
    public class RecebimentoNotaFiscalController : BaseController
    {
        private ViewNotaFiscalPagamentoRepository _viewNotaFiscalPagamentoRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private NotaFiscalPagamentoRepository _notaFiscalPagamentoRepository { get; set; }
        private PagamentoRepository _pagamentoRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private NotaFiscalPagamentoService _notaFiscalPagamentoService { get; set; }

        [BaseAuthorize("PAG07", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Listar(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {

            var results = _viewNotaFiscalPagamentoRepository.ListarComGrupo(filtro);


            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("PAG07", "DownloadArquivo")]
        [Transaction(TransactionAttributeType.Required)]
        public FileContentResult DownloadArquivo(long idArquivo, long idNotaFiscalPagamento, bool permissaoReceber)
        {
            var notaFiscalPagamento = _notaFiscalPagamentoRepository.FindById(idNotaFiscalPagamento);

            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Fazendo upload da nota fiscal: {0}", notaFiscalPagamento.NotaFiscal));
            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Nota fiscal: {0} | {1} | {2}", notaFiscalPagamento.Id, notaFiscalPagamento.NotaFiscal, notaFiscalPagamento.Situacao.AsString()));

            //if (notaFiscalPagamento.Situacao == SituacaoNotaFiscal.AguardandoProcessamento && permissaoReceber)
            //{
            //    _notaFiscalPagamentoService.MudarSituacao(notaFiscalPagamento, SituacaoNotaFiscal.AguardandoAvaliacao);
            //}
            var arquivo = _arquivoRepository.FindById(idArquivo);

            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Arquivo: {0} | {1}", arquivo.Id,arquivo.Nome));

            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        [BaseAuthorize("PAG07", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult MudarSituacao(ViewNotaFiscalPagamento model)
        {
            var json = new JsonResponse();
            var errors = new BusinessRuleException();
            try
            {
                if (model.SituacaoNotaFiscal.IsEmpty())
                {
                    errors.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Situacao)).Complete();
                    errors.AddField("nova_situacao");


                }
                errors.ThrowIfHasError();

                if (model.SituacaoNotaFiscal == SituacaoNotaFiscal.Reprovado && model.Motivo.IsEmpty())
                {
                    errors.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Motivo)).Complete();
                }
                errors.ThrowIfHasError();

                var notaFiscalPagamento = _notaFiscalPagamentoRepository.FindById(model.IdNotaFiscalPagamento.Value);
                notaFiscalPagamento.Motivo = model.Motivo;

                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Alterar Situação da nota fiscal: {0}", notaFiscalPagamento.NotaFiscal));
                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Nota fiscal: {0} | {1} | {2}", notaFiscalPagamento.Id, notaFiscalPagamento.NotaFiscal, notaFiscalPagamento.Situacao.AsString()));

                _notaFiscalPagamentoService.MudarSituacao(notaFiscalPagamento, model.SituacaoNotaFiscal);


                if (model.SituacaoNotaFiscal == SituacaoNotaFiscal.Reprovado)
                {
                    EnviarNotificacao(model.IdEmpresaVenda, model.PedidoSap);
                }

                json.Sucesso = true;
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSalvo,
                    model.NotaFiscal, GlobalMessages.Alterado));

            }
            catch (BusinessRuleException bre)
            {
                json.Mensagens.AddRange(bre.Errors);
                json.Campos.AddRange(bre.ErrorsFields);
                json.Sucesso = false;

            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        private void EnviarNotificacao(long idsEmpresaVenda, string pedido)
        {

            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idsEmpresaVenda);

            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = GlobalMessages.NotificacaoNotaReprovada_Titulo,
                    Conteudo = string.Format(GlobalMessages.NotificacaoNotaReprovada_Conteudo, pedido),
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    TipoNotificacao = TipoNotificacao.Lead,
                    Link = ProjectProperties.EvsBaseUrl + "/financeiro",
                    NomeBotao = GlobalMessages.IrParaFinanceiro,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };

                _notificacaoRepository.Save(notificacao);
            }
        }


        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult BaixarTodosDocumentos(List<long> idsNotaFiscalPagamento, bool permissaoReceber)
        {
            byte[] file = _notaFiscalPagamentoService.ExportarTodosDocumentos(idsNotaFiscalPagamento, permissaoReceber);
            string date = DateTime.Now.ToString("yyyyMMdd");
            return File(file, "application/zip", $"NotasFiscais_{date}.zip");
        }

        public JsonResult HistoricoNotaFiscal(long? Id, long? idEmpresaVenda, string pedido)
        {
            NotaFiscalPagamento dto = new NotaFiscalPagamento();

            if (Id.HasValue())
            {
                var dt = _notaFiscalPagamentoRepository.FindById(Id.Value);
                dto = dt;
            }

            if(idEmpresaVenda.HasValue() && pedido.HasValue() && dto.DataPedido.IsEmpty())
            {
                DateTime DataPedido = _pagamentoRepository.BuscarDataCriacaoPagamento(idEmpresaVenda.Value, pedido);
                dto.DataPedido = DataPedido;
            }

            var auditViewModelHtml = RenderRazorViewToString("~/Views/RecebimentoNotaFiscal/_HistoricoNotaFiscal.cshtml", dto, false);

            var json = new JsonResponse();
            json.Objeto = auditViewModelHtml;
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [BaseAuthorize("PAG07", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {

            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _notaFiscalPagamentoService.ExportarRecebimentoNotaFiscal(modifiedRequest, filtro);
            string nomeArquivo = "RecebimentoNotaFiscal";
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        [BaseAuthorize("PAG07", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {

            byte[] file = _notaFiscalPagamentoService.ExportarRecebimentoNotaFiscal(request, filtro);
            string nomeArquivo = "RecebimentoNotaFiscal";
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public ActionResult ExibirNotaFiscal(NotaFiscalPagamentoPdfDTO filtro)
        {
            var response = new JsonResponse();

            try
            {
                var notaFiscal = _notaFiscalPagamentoService.ExibirNotaFiscal(filtro);
                response.Objeto = RenderRazorViewToString("_NotaFiscal", notaFiscal, false);
                response.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public FileContentResult DownloadPdf(NotaFiscalPagamentoPdfDTO dto)
        {
            var fileName = "CabecalhoNF.png";
            var TargetPath = @"/Static/images/";
            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
            byte[] logo = System.IO.File.ReadAllBytes(path);
            var arquivo = _notaFiscalPagamentoService.GerarPdf(dto, logo);

            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        [BaseAuthorize("PAG07", "AtualizarNumeroNF")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AtualizarNumeroNF(long idNf, string numero)
        {
            var response = new JsonResponse();

            try
            {
                var nota = _notaFiscalPagamentoService.AtualizarNumeroNf(idNf, numero);
                response.Sucesso = true;
                response.Mensagens.Add(String.Format(GlobalMessages.RegistroSucesso, nota.NotaFiscal, GlobalMessages.Alterado.ToLower()));
            }
            catch (BusinessRuleException ex)
            {
                response.Mensagens.Add(ex.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}