using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS19")]
    public class FinanceiroController : BaseController
    {
        private ViewNotaFiscalPagamentoRepository _viewNotaFiscalPagamentoRepository { get; set; }
        private NotaFiscalPagamentoRepository _notaFiscalPagamentoRepository { get; set; }
        private NotaFiscalPagamentoService _notaFiscalPagamentoService { get; set; }
        private PagamentoRepository _pagamentoRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private FechamentoContabilRepository _fechamentoContabilRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private NotaFiscalPagamentoOcorrenciaService notaFiscalPagamentoOcorrenciaService { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private EnderecoFornecedorRepository _enderecoFornecedorRepository { get; set; }
        public ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository { get; set; }
        public OcorrenciasMidasRepository _ocorrenciasMidasRepository { get; set; }

        public StaticResourceService _staticResourceService { get; set; }

        public ActionResult Index()
        {
            var fechamentoDto = new FechamentoContabilDto();

            var fechamentoContabil = _fechamentoContabilRepository.FechamentoContabilVigente();

            if (fechamentoContabil.HasValue())
            {
                fechamentoDto.EmFechamentoContabil = fechamentoContabil.HasValue();
                fechamentoDto.InicioFechamento = fechamentoContabil.InicioFechamento;
                fechamentoDto.TerminoFechamento = fechamentoContabil.TerminoFechamento;
            }

            return View(fechamentoDto);
        }

        public ActionResult Listar(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;
            filtro.Faturado = 1;

            var results = _viewNotaFiscalPagamentoRepository.ListarComGrupo(filtro);

            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult UploadNotaFiscal(ViewNotaFiscalPagamento documento, HttpPostedFileBase file)
        {
            
            var jsonResponse = new JsonResponse();
            try
            {
                var errors = new BusinessRuleException();
                var idEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;
                if (file == null || documento == null ||
                    documento.IdPagamento.IsEmpty())
                {
                    errors.AddError(GlobalMessages.MsgNenhumArquivoSelecionado).Complete();
                }

                if (documento.NotaFiscal.IsEmpty())
                {
                    errors.AddError(string.Format(GlobalMessages.FavorInformar, GlobalMessages.NumeroNotaFiscalP)).Complete();
                }
                else
                {
                    if (_pagamentoRepository.CheckExistNotaFiscal(idEmpresaVenda, documento.NotaFiscal, documento.PedidoSap))
                    {
                        errors.AddError(string.Format(GlobalMessages.MsgErroRegistroExistente, GlobalMessages.NotaFiscal, GlobalMessages.Numero.ToLower())).Complete();
                    }
                }

                errors.ThrowIfHasError();

                var pagamento = _pagamentoRepository.BuscarPagamentoPorPedidoSap(idEmpresaVenda, documento.PedidoSap);

                if (pagamento == null)
                {
                    errors.AddError(GlobalMessages.RegistroNaoEncontrado)
                        .WithParams(GlobalMessages.Pagamento, documento.IdPagamento.ToString()).Complete();
                }

                errors.ThrowIfHasError();
                               
                // Se houver documento anexado, ele deve ser utilizado novamente e não criado um novo
                var notaFiscalPagamento = new NotaFiscalPagamento();
                var Valor = pagamento[0].ValorPagamento.Value.ToString("F");
                Valor = Valor.Replace(",", "");

                var nomeArquivo = string.Format("{0} - {1}.pdf",
                    documento.PedidoSap.ToUpper(),
                    SessionAttributes.Current().EmpresaVenda.Estado.ToUpper());

                var arquivo = _arquivoService.CreateFile(file, nomeArquivo);

                _arquivoService.PreencherMetadadosDePdf(ref arquivo);
                
                if (documento.IdNotaFiscalPagamento.HasValue())
                {
                    notaFiscalPagamento = _notaFiscalPagamentoRepository.FindById(documento.IdNotaFiscalPagamento.Value);
                }
                notaFiscalPagamento.DataPedido = pagamento.First().DataPedidoSap;

                //GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Fazendo upload da nota fiscal: {0}", documento.NotaFiscal));
                //GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Proposta: {0} | {1}", pagamento.First().Proposta.Id, pagamento.First().Proposta.CodigoProposta));
                //GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Arquivo: {0}", nomeArquivo));
                //GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Pagamento:{0} | Pedido SAP: {1} | RC: {2} | {3}", pagamento.First().Id, pagamento.First().PedidoSap,pagamento.First().ReciboCompra,pagamento.First().TipoPagamento.AsString()));
                //GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Nota fiscal: {0} | {1} | {2}", notaFiscalPagamento.Id, notaFiscalPagamento.NotaFiscal, notaFiscalPagamento.Situacao.AsString()));


                notaFiscalPagamento = _notaFiscalPagamentoService.SalvarPorFinanceiroPortal(notaFiscalPagamento, documento.NotaFiscal, arquivo);

                #region Cruzamento Midas

                //var viewRelUnificado = _viewRelatorioVendaUnificadoRepository.Queryable().Where(x => x.IdNotaFiscalPagamento == notaFiscalPagamento.Id).FirstOrDefault();

                //var fornecedor = _enderecoFornecedorRepository.Queryable()
                //        .Where(x => x.CodigoFornecedor.Equals(viewRelUnificado.CodigoEmpresa))
                //        .Where(x => x.Estado.Equals(viewRelUnificado.Regional))
                //        .FirstOrDefault();

                //var empreendimento = _empreendimentoRepository.FindById(viewRelUnificado.IdEmpreendimento);

                //var matchPagamento = new bool();

                //if (empreendimento.HasValue())
                //{
                //    if (fornecedor.HasValue())
                //    {
                //        matchPagamento = _ocorrenciasMidasRepository.MatchPagamentoOcorrencias(viewRelUnificado.CnpjEmpresaVenda, fornecedor.Cnpj, notaFiscalPagamento);
                //    }
                //    else
                //    {
                //        matchPagamento = _ocorrenciasMidasRepository.MatchPagamentoOcorrencias(viewRelUnificado.CnpjEmpresaVenda, empreendimento.CNPJ, notaFiscalPagamento);
                //    }

                //    if (matchPagamento)
                //    {
                //        notaFiscalPagamento.Situacao = SituacaoNotaFiscal.AguardandoAvaliacao;
                //        _notaFiscalPagamentoRepository.Save(notaFiscalPagamento);
                //    }
                //}

                #endregion

                //GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Nota fiscal: {0} | {1} | {2}", notaFiscalPagamento.Id, notaFiscalPagamento.NotaFiscal, notaFiscalPagamento.Situacao.AsString()));

                foreach (var pag in pagamento)
                {
                    pag.NotaFiscalPagamento = notaFiscalPagamento;
                    _pagamentoRepository.Save(pag);

                }
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.NotaFiscalCarregadoSucesso,
                    documento.NotaFiscal));
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;

                foreach (var e in bre.Errors)
                {
                    GenericFileLogUtil.DevLogWithDateOnBegin(e);
                }
            }catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
                jsonResponse.Mensagens.Add(ex.Message);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }


        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;

            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = _notaFiscalPagamentoService.Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.Financeiro;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;

            byte[] file = _notaFiscalPagamentoService.Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.Financeiro;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
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
        public ActionResult ExibirDadosNotaFiscal(NotaFiscalPagamentoPdfDTO dto)
        {
            var json = new JsonResponse();

            var fileName = "CabecalhoNF.png";
            var TargetPath = @"/Static/images/";
            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
            byte[] logo = System.IO.File.ReadAllBytes(path);

            var arquivo = _notaFiscalPagamentoService.GerarPdf(dto, logo);
            arquivo.Hash = HashUtil.SHA256(arquivo.Content);
            arquivo.FileExtension = ".pdf";
            var resourcePath = _staticResourceService.UploadFile(arquivo);

            var urlArquivo = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);

            var arquivoDto = new ArquivoDto();
            arquivoDto.Url = urlArquivo;
            json.Objeto = arquivoDto;

            return Json(json);
        }
        public ActionResult BuscarTotal(FiltroPagamentoDTO filtro)
        {
            filtro.IdEmpresaVenda = SessionAttributes.Current().EmpresaVenda.Id;
            var result = _viewNotaFiscalPagamentoRepository.Listar(filtro);


            decimal somaValores = 0;
            if (!result.IsEmpty())
            {
                somaValores = result.Sum(x => x.ValorAPagar);
            }


            var dto = new TotalDTO();
            dto.Total = String.Format(new CultureInfo("pt-BR"), "{0:C}", somaValores);

            return Json(dto);
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
            catch(BusinessRuleException ex)
            {
                response.Mensagens.Add(ex.Message);
            }            

            return Json(response, JsonRequestBehavior.AllowGet);
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

            var auditViewModelHtml = RenderRazorViewToString("~/Views/Financeiro/_HistoricoNotaFiscal.cshtml", dto, false);

            var json = new JsonResponse();
            json.Objeto = auditViewModelHtml;
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}