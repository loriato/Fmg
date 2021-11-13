using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Zeus;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PagamentoService : BaseService
    {
        public PagamentoRepository _pagamentoRepository { get; set; }
        public ViewRelatorioComissaoRepository _viewRelatorioComissaoRepository { get; set; }
        public ViewPagamentoRepository _viewPagamentoRepository { get; set; }
        public RequisicaoCompraService _requisicaoCompraService { get; set; }
        public RequisicaoCompraSapService _requisicaoCompraSapService { get; set; }
        private RequisicaoCompraDTOValidator _requisicaoDTOValidator { get; set; }
        public ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository { get; set; }
        public NumeroPedidoSapRepository _numeroPedidoSapRepository { get; set; }
        public ZeusApiService _zeusApiService { get; set; }
        public void SalvarPagamento(Pagamento pagamento)
        {
            var bre = new BusinessRuleException();

            var result = new PagamentoValidator(_pagamentoRepository).Validate(pagamento);

            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();

            if (!pagamento.Id.IsEmpty())
            {
                var pag = _pagamentoRepository.FindById(pagamento.Id);
                pag.Situacao = Situacao.Cancelado;
                _pagamentoRepository.Save(pag);
            }

            var pagNovo = new Pagamento
            {
                Situacao = Situacao.Ativo,
                PedidoSap = pagamento.PedidoSap,
                Proposta = new PropostaSuat { Id = pagamento.Proposta.Id },
                TipoPagamento = pagamento.TipoPagamento,
                ValorPagamento = pagamento.ValorPagamento,
                EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = pagamento.EmpresaVenda.Id }
            };
            _pagamentoRepository.Save(pagNovo);

        }
        public bool RemoverPagamento(long idPagamento)
        {
            var bre = new BusinessRuleException();
            var pagamento = _pagamentoRepository.FindById(idPagamento);
            var itemPedido = _numeroPedidoSapRepository.FindNumeroItemDocCompraPorRequisicaoCompra(pagamento.ReciboCompra);

            var EmRevercao = _viewRelatorioVendaUnificadoRepository.Queryable()
                .Where(x => x.EmReversao == true || x.PassoAtual == "Prop. Cancelada")
                .Where(x => x.IdPagamento == idPagamento).HasValue();

            try
            {
                if (!pagamento.IsNull())
                {
                    if (itemPedido.HasValue() && EmRevercao)
                    {
                        var retorno = CancelarPagamentoEmRevercao(itemPedido, pagamento.PedidoSap);
                        if (!retorno.Mensagem.Contains("sucesso"))
                        {
                            return false;
                        }
                    }
                    _pagamentoRepository.RemoverPagamento(pagamento);
                    
                }
            }
            catch (GenericADOException ex)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(ex))
                {
                    _session.Transaction.Rollback();
                    bre.AddError(string.Format(GlobalMessages.RemovidoSemSucesso, pagamento.ChaveCandidata()));
                }
            }
            finally
            {
                bre.ThrowIfHasError();
            }
            return true;
        }

        public CancelarPagamentoResponseDTO CancelarPagamentoEmRevercao(string item, string pedido)
        {
            
            var respostaZeus = _zeusApiService.CancelarPagamento(pedido, item);

            return respostaZeus;
        }

        public List<PagamentoDTO> ListarPagamentos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var bre = new BusinessRuleException();

            var dados = _viewPagamentoRepository.ListarPagamentos(request, filtro);

            var evs = new Dictionary<long, string>();

            foreach (var res in dados.records)
            {
                if (!evs.ContainsKey(res.IdEmpresaVenda))
                {
                    evs.Add(res.IdEmpresaVenda, res.NomeEmpresaVenda);
                }
            }

            if (evs.IsEmpty())
            {
                bre.AddError(string.Format("Não há propostas para o periodo: {0} a {1}", filtro.PeriodoDe.ToDate(), filtro.PeriodoAte.ToDate())).Complete();
                bre.ThrowIfHasError();
            }

            var lista = new List<PagamentoDTO>();

            foreach (var ev in evs)
            {
                var propostas = dados.records.ToList().Where(x => x.IdEmpresaVenda == ev.Key);

                var propostasPagas = propostas.Where(x => x.Pago == true);
                decimal totalPago = 0;
                if (propostasPagas.Any())
                {
                    totalPago = propostasPagas.Sum(x => x.ValorAPagar);
                }

                //var totalPago = _pagamentoRepository.Queryable()
                //    .Where(x => x.EmpresaVenda.Id == ev.Key)
                //    .Where(x => x.Situacao == Situacao.Ativo)
                //    .Where(x => x.ValorPagamento != null)
                //    .Select(x => x.ValorPagamento)
                //    .Sum(x => x);

                var pagamento = new PagamentoDTO
                {
                    IdEmpresaVenda = ev.Key,
                    NomeEmpresaVenda = ev.Value,
                    Propostas = propostas.OrderBy(x => x.CodigoProposta).AsQueryable(),
                    TotalAPagar = propostas.Sum(x => x.ValorAPagar),
                    TotalPago = totalPago
                };

                lista.Add(pagamento);
            }

            return lista;
        }
                
        public string RenderFaixa(ViewPagamento full)
        {
            switch (full.Tipologia)
            {
                case Tipologia.PNE:
                    return "PNE";
                case Tipologia.FaixaUmMeio:
                    return "F 1,5";
                case Tipologia.FaixaDois:
                    return "F 2,0";
            }

            if (full.FaixaUmMeio)
            {
                return "F 1,5";
            }

            return "F 2,0";
        }
        private string[] GetHeader(string NomeEmpresaVenda)
        {
            IList<string> header = new List<string>
            {
                    NomeEmpresaVenda,
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    ""
            };
            return header.ToArray();
        }

        public string RegraPagamento(string regraPagamento, TipoPagamento tipo)
        {
            switch (tipo)
            {
                case TipoPagamento.KitCompleto:
                    return regraPagamento + "% Kit Completo";
                case TipoPagamento.Repasse:
                    return regraPagamento + "% Repasse";
                case TipoPagamento.Conformidade:
                    return regraPagamento + "% Conformidade";
                default:
                    break;
            }
            return "";
        }

        public void SalvarRCPagamento(ViewPagamento viewPagamento, string requisicaoCompra)
        {
            
            var pagamento = _pagamentoRepository.FindById(viewPagamento.IdPagamento);
            var pagamentoNovo = new Pagamento();

            if (pagamento.HasValue())
            {
                pagamentoNovo = pagamento;
            }
            else
            {
                pagamentoNovo.ValorPagamento = viewPagamento.ValorAPagar;
                pagamentoNovo.TipoPagamento = viewPagamento.TipoPagamento;
                pagamentoNovo.Proposta = new PropostaSuat { Id = viewPagamento.IdProposta };
                pagamentoNovo.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = viewPagamento.IdEmpresaVenda };
            }

            if(pagamentoNovo.ReciboCompra.IsEmpty() && requisicaoCompra.HasValue())
            {
                pagamentoNovo.DataRequisicaoCompra = DateTime.Now;
            }

            pagamentoNovo.StatusIntegracaoSap = StatusIntegracaoSap.AguardandoPedido;
            pagamentoNovo.Situacao = Situacao.Ativo;
            pagamentoNovo.ReciboCompra = requisicaoCompra;
            _pagamentoRepository.Save(pagamentoNovo);

        }

        #region Exportar
        public byte[] ExportarPagamentos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var pagamentos = _viewPagamentoRepository.ListarPagamentos(request, filtro).records.ToList();

            var evs = new Dictionary<long, string>();

            foreach(var pag in pagamentos)
            {
                if (evs.ContainsKey(pag.IdEmpresaVenda))
                {
                    continue;
                }

                evs.Add(pag.IdEmpresaVenda, pag.NomeEmpresaVenda);
            }

            ExcelUtil excel = ExcelUtil.NewInstance(27)
                .NewSheet(DateTime.Now.ToString());

            foreach (var ev in evs)
            {
                var propostas = pagamentos.Where(x => x.IdEmpresaVenda == ev.Key).OrderBy(x=>x.CodigoProposta).ToList();
                
                if (propostas.IsEmpty())
                {
                    continue;
                }

                var totalAPagar = propostas.Sum(x => x.ValorAPagar);
                var totalPago = propostas.Where(x => x.Pago == true).Sum(x => x.ValorAPagar);

                excel.WithHeader(GetHeader(ev.Value));

                excel
                    .CreateCellValue(GlobalMessages.Central).Width(20)
                    .CreateCellValue(GlobalMessages.UF).Width(20)
                    .CreateCellValue(GlobalMessages.Empreendimento).Width(20)
                    .CreateCellValue(GlobalMessages.Regional).Width(20)
                    .CreateCellValue(GlobalMessages.Proposta).Width(20)
                    .CreateCellValue(GlobalMessages.Cliente).Width(20)
                    .CreateCellValue(GlobalMessages.Situacao).Width(20)
                    .CreateCellValue(GlobalMessages.DataVenda).Width(20)
                    .CreateCellValue(GlobalMessages.StatusRepasse).Width(20)
                    .CreateCellValue(GlobalMessages.StatusConformidade).Width(20)
                    .CreateCellValue(GlobalMessages.SituacaoNotaFiscal).Width(20)
                    .CreateCellValue(GlobalMessages.FaixaPagamento).Width(20)
                    .CreateCellValue(GlobalMessages.Percentual).Width(20)
                    .CreateCellValue(GlobalMessages.PorcentagemComissao).Width(20)
                    .CreateCellValue(GlobalMessages.DataAptidao).Width(20)
                    .CreateCellValue(GlobalMessages.ValorSemPremiada).Width(20)
                    .CreateCellValue(GlobalMessages.ValorPagar).Width(20)
                    .CreateCellValue(GlobalMessages.CodigoRegra).Width(20)
                    .CreateCellValue(GlobalMessages.EmReversao).Width(20)
                    .CreateCellValue(GlobalMessages.RC).Width(20)
                    .CreateCellValue(GlobalMessages.DataRC).Width(20)
                    .CreateCellValue(GlobalMessages.NumeroPedido).Width(20)
                    .CreateCellValue(GlobalMessages.DataPedidoSap).Width(20)
                    .CreateCellValue(GlobalMessages.MIRO).Width(20)
                    .CreateCellValue(GlobalMessages.MIGO).Width(20)
                    .CreateCellValue(GlobalMessages.MIR7).Width(20)
                    .CreateCellValue(GlobalMessages.NotaFiscal).Width(20)
                    .CreateCellValue(GlobalMessages.ChamadoPgto).Width(20)
                    .CreateCellValue(GlobalMessages.PrevisaoPgto).Width(20)
                    .CreateCellValue(GlobalMessages.Pagamento).Width(20)
                    .CreateCellValue(GlobalMessages.SituacaoPagamento).Width(20)
                    .CreateCellValue(GlobalMessages.Faturado)
                    ;

                //informação das propostas aqui
                foreach(var proposta in propostas)
                {
                    excel
                        .CreateCellValue(proposta.NomeLoja).Width(20)
                        .CreateCellValue(proposta.Estado).Width(20)
                        .CreateCellValue(proposta.NomeEmpreendimento).Width(20)
                        .CreateCellValue(proposta.Regional).Width(20)
                        .CreateCellValue(proposta.CodigoProposta).Width(20)
                        .CreateCellValue(proposta.NomeCliente).Width(20)
                        .CreateCellValue(proposta.PassoAtual).Width(20)
                        .CreateDateTimeCell(proposta.DataVenda).Width(20)
                        .CreateCellValue(proposta.StatusRepasse).Width(20)
                        .CreateCellValue(proposta.StatusConformidade ? "Sim" : "Não").Width(20)
                        .CreateCellValue(proposta.SituacaoNotaFiscal.AsString()).Width(20)
                        .CreateCellValue(RenderFaixa(proposta)).Width(20)
                        .CreateCellValue(proposta.Percentual + "%").Width(20)
                        .CreateCellValue(RegraPagamento(proposta.RegraPagamento, proposta.TipoPagamento)).Width(20)
                        .CreateDateTimeCell(proposta.DataComissao).Width(20)
                        .CreateMoneyCell(Convert.ToDecimal(proposta.ValorSemPremiada.ToString("F"))).Width(20)
                        .CreateMoneyCell(Convert.ToDecimal(proposta.ValorAPagar.ToString("F"))).Width(20)
                        .CreateCellValue(proposta.CodigoRegraComissao).Width(20)
                        .CreateCellValue(proposta.EmReversao ? "Sim" : "Não").Width(20)
                        .CreateCellValue(proposta.ReciboCompra).Width(20)
                        .CreateDateTimeCell(proposta.DataRequisicaoCompra).Width(20)
                        .CreateCellValue(proposta.PedidoSap).Width(20)
                        .CreateDateTimeCell(proposta.DataPedidoSap).Width(20)
                        .CreateCellValue(proposta.MIRO).Width(20)
                        .CreateCellValue(proposta.MIGO).Width(20)
                        .CreateCellValue(proposta.MIR7).Width(20)
                        .CreateCellValue(proposta.NotaFiscal).Width(20)
                        .CreateCellValue(proposta.ChamadoPagamento).Width(20)
                        .CreateDateTimeCell(proposta.DataPrevisaoPagamento).Width(20)
                        .CreateCellValue(proposta.Pago ? "Sim" : "Não").Width(20)
                        .CreateCellValue(proposta.EmReversao ? "Cancelado" : " ").Width(20)
                        .CreateCellValue(proposta.Faturado?GlobalMessages.Sim:GlobalMessages.Nao);
                }
                //informação das propostas aqui

                excel
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue(GlobalMessages.TotalPagar).Width(20)
                    .CreateMoneyCell(Convert.ToDecimal(totalAPagar.ToString("F"))).Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue(GlobalMessages.TotalPago)
                    .CreateMoneyCell(Convert.ToDecimal(totalPago.ToString("F")))
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20);

                excel
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20)
                    .CreateCellValue("").Width(20);

            }

            
            excel.Close();
            return excel.DownloadFile();
        }
        #endregion

        #region Gerar Requisição de compra em lote

        public RequisicaoCompraDTO MontarRequisicaoCompra(ViewPagamento pagamento,ItemRequisicaoDTO itemRequisicao)
        {
            itemRequisicao.TextoBreve = string.Format(itemRequisicao.TextoBreve, pagamento.CodigoProposta);
            itemRequisicao.DataLiberacao = DateTime.Now;
            itemRequisicao.DataRemessaItem = DateTime.Now;
            itemRequisicao.DataSolicitacao = DateTime.Now;
            itemRequisicao.PrecoUnidade = 1;

            itemRequisicao.CentroDeCusto = pagamento.DivisaoEmpreendimento;
            itemRequisicao.Preco = Convert.ToDouble(pagamento.ValorAPagar);

            itemRequisicao.Preco = Math.Round(itemRequisicao.Preco, 2);

            itemRequisicao.FornecedorPretendido = pagamento.CodigoFornecedor;

            ContabilizacaoRequisicaoDTO contabilizacaoRequisicao = new ContabilizacaoRequisicaoDTO();

            var codigo = "000";
            if (pagamento.CodigoEmpresa.ToUpper() == "0070")
            {
                codigo = "290";
            }
            else if (pagamento.CodigoEmpresa.ToUpper() != "0070")
            {
                codigo = "291";
            }
            var numeroOrdem = pagamento.DivisaoEmpreendimento.Substring(0, pagamento.DivisaoEmpreendimento.Length - 1) + "I" + codigo;
            contabilizacaoRequisicao.NumeroOrdem = numeroOrdem;

            contabilizacaoRequisicao.Divisao = pagamento.DivisaoEmpreendimento;

            var requisicao = new RequisicaoCompraDTO();

            requisicao.ItemRequisicao = itemRequisicao;
            requisicao.ContabilizacaoRequisicao = contabilizacaoRequisicao;
            requisicao.Pagamento = pagamento;

            return requisicao;
        }

        public List<string> GerarRequisicaoCompra(List<ViewPagamento> pagamentos,DadosAuditoriaDTO dadosAuditoria)
        {
            var mensagens = new List<string>();
            
            foreach(var pagamento in pagamentos)
            {
                var parametroRequisicao = new ItemRequisicaoDTO();
                _requisicaoCompraService.GetParametros(parametroRequisicao);

                var requisicao = MontarRequisicaoCompra(pagamento, parametroRequisicao);

                requisicao.ContabilizacaoRequisicao.NumeroItem = requisicao.ItemRequisicao.NumeroItem;
                requisicao.ContabilizacaoRequisicao.Quantidade = requisicao.ItemRequisicao.Quantidade;

                var count = pagamentos.AsQueryable().Where(x => x.IdPagamento == requisicao.Pagamento.IdPagamento)
                    .Where(x => x.TipoPagamento == requisicao.Pagamento.TipoPagamento)
                    .Where(x => x.IdProposta == requisicao.Pagamento.IdProposta)
                    .Where(x=>x.IdEmpresaVenda==requisicao.Pagamento.IdEmpresaVenda)
                    .Count();

                if (count >= 2)
                {
                    GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Existe duas ou mais proposta {0} iguais nessa lista para geração de RC ", requisicao.Pagamento.CodigoProposta));

                    mensagens.Add(string.Format("Existe duas ou mais proposta {0} iguais nessa lista para geração de RC ", requisicao.Pagamento.CodigoProposta));
                    continue;
                }

                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Iniciando requisição no ZEUS as {0} para proposta {1}", DateTime.Now, pagamento.ChaveCandidata()));

                var result = ZeusService.GerarRC(requisicao.ItemRequisicao, requisicao.ContabilizacaoRequisicao, dadosAuditoria);

                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Resultados da requisição para proposta {0}", pagamento.CodigoProposta));

                foreach (var item in result)
                {
                    GenericFileLogUtil.DevLogWithDateOnBegin(item.ToString());

                    var requisicaoCompraSap = new RequisicaoCompraSap();
                    requisicaoCompraSap.Numero = item.numero;
                    requisicaoCompraSap.Status = item.status;
                    requisicaoCompraSap.Proposta = new PropostaSuat { Id = requisicao.Pagamento.IdProposta };
                    requisicaoCompraSap.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = requisicao.Pagamento.IdEmpresaVenda };
                    requisicaoCompraSap.TipoPagamento = requisicao.Pagamento.TipoPagamento;
                    requisicaoCompraSap.Texto = item.texto;
                    requisicaoCompraSap.NumeroGerado = item.status.IsEmpty();

                    if (requisicaoCompraSap.NumeroGerado)
                    {
                        GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Salvando requisição de compra {0}", requisicaoCompraSap.ToString()));
                        _requisicaoCompraSapService.Salvar(requisicaoCompraSap);
                        SalvarRCPagamento(requisicao.Pagamento, item.numero);
                        mensagens.Add(string.Format("Requisição compra número {0} gerada para a proposta {1}", item.numero, requisicao.Pagamento.CodigoProposta));
                    }
                    else
                    {
                        GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("{0}. referente a proposta {1}", item.status, requisicao.Pagamento.CodigoProposta));

                        mensagens.Add(string.Format("{0}. referente a proposta {1}", item.status, requisicao.Pagamento.CodigoProposta));                        
                    }
                }
            }

            return mensagens;
        }

        public List<string> GerarRequisicaoCompra(List<ViewPagamento> pagamentos, DadosAuditoriaDTO dadosAuditoria, ISession session, string diretorio, ref ImportTaskDTO importTask)
        {
            var mensagens = new List<string>();

            try
            {
                _session = session;

                _requisicaoCompraService._parametroRequisicaoCompraRepository=new ParametroRequisicaoCompraRepository()
                {
                    _session = session
                };
                _pagamentoRepository = new PagamentoRepository()
                {
                    _session = session
                };

                _requisicaoCompraSapService._requisicaoCompraSapRepository = new RequisicaoCompraSapRepository()
                {
                    _session = session
                };
                                
                //InternalProcess(diretorio, ref importTask);

                importTask.AppendLog("Execução de serviço iniciada");
                importTask.AppendLog(string.Format("Total de pagamentos a serem processados: {0}",pagamentos.Count));
                importTask.TotalLines = pagamentos.Count;

                var transaction = _session.BeginTransaction();

                foreach (var pagamento in pagamentos)
                {
                    transaction = _session.BeginTransaction();
                    importTask.CurrentLine++;

                    var parametroRequisicao = new ItemRequisicaoDTO();
                    _requisicaoCompraService.GetParametros(parametroRequisicao);

                    var requisicao = MontarRequisicaoCompra(pagamento, parametroRequisicao);

                    requisicao.ContabilizacaoRequisicao.NumeroItem = requisicao.ItemRequisicao.NumeroItem;
                    requisicao.ContabilizacaoRequisicao.Quantidade = requisicao.ItemRequisicao.Quantidade;

                    var count = pagamentos.AsQueryable().Where(x => x.IdPagamento == requisicao.Pagamento.IdPagamento)
                        .Where(x => x.TipoPagamento == requisicao.Pagamento.TipoPagamento)
                        .Where(x => x.IdProposta == requisicao.Pagamento.IdProposta)
                        .Where(x => x.IdEmpresaVenda == requisicao.Pagamento.IdEmpresaVenda)
                        .Count();

                    if (count >= 2)
                    {
                        var erro = string.Format("Existe duas ou mais proposta {0} iguais nessa lista para geração de RC ", requisicao.Pagamento.CodigoProposta);
                        LogError(ref importTask, erro);
                        mensagens.Add(erro);
                        continue;
                    }

                    var result = ZeusService.GerarRC(requisicao.ItemRequisicao, requisicao.ContabilizacaoRequisicao, dadosAuditoria);

                    foreach (var item in result)
                    {
                        
                        var requisicaoCompraSap = new RequisicaoCompraSap();
                        requisicaoCompraSap.Numero = item.numero;
                        requisicaoCompraSap.Status = item.status;
                        requisicaoCompraSap.Proposta = new PropostaSuat { Id = requisicao.Pagamento.IdProposta };
                        requisicaoCompraSap.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = requisicao.Pagamento.IdEmpresaVenda };
                        requisicaoCompraSap.TipoPagamento = requisicao.Pagamento.TipoPagamento;
                        requisicaoCompraSap.Texto = item.texto;
                        requisicaoCompraSap.NumeroGerado = item.status.IsEmpty();

                        if (requisicaoCompraSap.NumeroGerado)
                        {
                            _requisicaoCompraSapService.Salvar(requisicaoCompraSap);
                            SalvarRCPagamento(requisicao.Pagamento, item.numero);
                            mensagens.Add(string.Format("Requisição compra número {0} gerada para a proposta {1}", item.numero, requisicao.Pagamento.CodigoProposta));

                            var msg = string.Format("Requisição compra número {0} gerada para a proposta {1}", item.numero, requisicao.Pagamento.CodigoProposta);
                            importTask.AppendLog(msg);
                        }
                        else
                        {
                            var erro = string.Format("{0}. referente a proposta {1}", item.status, requisicao.Pagamento.CodigoProposta);
                            LogError(ref importTask, erro);

                            mensagens.Add(string.Format("{0}. referente a proposta {1}", item.status, requisicao.Pagamento.CodigoProposta));
                        }
                    }

                    importTask.SuccessCount++;

                    transaction.Commit();
                }

                if (transaction.IsActive)
                {
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                
                importTask.Error = e.Message;
                importTask.End = DateTime.Now;
                ExceptionLogger.LogException(e);
            }
            finally
            {
                string targetFileLog = string.Format("{0}{1}{2}-log.txt", diretorio, Path.DirectorySeparatorChar,
                    importTask.TaskId);
                importTask.AppendLog(String.Format("Persistindo log em {0}, com identificador {1}",
                    Environment.MachineName, importTask.TaskId));
                File.WriteAllText(targetFileLog, importTask.FullLog.ToString());

                if (_session != null && _session.IsOpen)
                {
                    _session.Close();
                }

            }

            importTask.End = DateTime.Now;

            return mensagens;
        }

        private void LogError(ref ImportTaskDTO importTask,string details)
        {
            importTask.IncrementError();
            importTask.AppendLog(details);
        }
        #endregion

        #region Pagamento Unificado
        public List<string> GerarRequisicaoCompra(List<ViewRelatorioVendaUnificado> vendas, DadosAuditoriaDTO dadosAuditoria, ISession session, string diretorio, ref ImportTaskDTO importTask)
        {
            var mensagens = new List<string>();

            try
            {
                _session = session;

                _requisicaoCompraService._parametroRequisicaoCompraRepository = new ParametroRequisicaoCompraRepository()
                {
                    _session = session
                };
                _pagamentoRepository = new PagamentoRepository()
                {
                    _session = session
                };

                _requisicaoCompraSapService._requisicaoCompraSapRepository = new RequisicaoCompraSapRepository()
                {
                    _session = session
                };

                //InternalProcess(diretorio, ref importTask);

                importTask.AppendLog("Execução de serviço iniciada");
                importTask.AppendLog(string.Format("Total de pagamentos a serem processados: {0}", vendas.Count));
                importTask.TotalLines = vendas.Count;


                var transaction = _session.BeginTransaction();

                foreach (var venda in vendas)
                {
                    try
                    {
                        ViewPagamento pagamento = ConvertToPagamento(venda);

                        transaction = _session.BeginTransaction();

                        var bre = new BusinessRuleException();

                        importTask.CurrentLine++;

                        var parametroRequisicao = new ItemRequisicaoDTO();
                        _requisicaoCompraService.GetParametros(parametroRequisicao);

                        var requisicao = MontarRequisicaoCompra(pagamento, parametroRequisicao);

                        requisicao.ContabilizacaoRequisicao.NumeroItem = requisicao.ItemRequisicao.NumeroItem;
                        requisicao.ContabilizacaoRequisicao.Quantidade = requisicao.ItemRequisicao.Quantidade;

                        var count = vendas.AsQueryable().Where(x => x.IdPagamento == requisicao.Pagamento.IdPagamento)
                            .Where(x => x.TipoPagamento == requisicao.Pagamento.TipoPagamento)
                            .Where(x => x.IdProposta == requisicao.Pagamento.IdProposta)
                            .Where(x => x.IdEmpresaVenda == requisicao.Pagamento.IdEmpresaVenda)
                            .Count();

                        if (count >= 2)
                        {
                            var erro = string.Format("Existe duas ou mais proposta {0} iguais nessa lista para geração de RC ", requisicao.Pagamento.CodigoProposta);
                            LogError(ref importTask, erro);
                            mensagens.Add(erro);
                            continue;
                        }

                        var validate = _requisicaoDTOValidator.Validate(requisicao);
                        bre.WithFluentValidation(validate);
                        bre.ThrowIfHasError();

                        var result = ZeusService.GerarRC(requisicao.ItemRequisicao, requisicao.ContabilizacaoRequisicao, dadosAuditoria);

                        foreach (var item in result)
                        {

                            var requisicaoCompraSap = new RequisicaoCompraSap();
                            requisicaoCompraSap.Numero = item.numero;
                            requisicaoCompraSap.Status = item.status;
                            requisicaoCompraSap.Proposta = new PropostaSuat { Id = requisicao.Pagamento.IdProposta };
                            requisicaoCompraSap.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = requisicao.Pagamento.IdEmpresaVenda };
                            requisicaoCompraSap.TipoPagamento = requisicao.Pagamento.TipoPagamento;
                            requisicaoCompraSap.Texto = item.texto;
                            requisicaoCompraSap.NumeroGerado = item.status.IsEmpty();

                            if (requisicaoCompraSap.NumeroGerado)
                            {
                                _requisicaoCompraSapService.Salvar(requisicaoCompraSap);
                                SalvarRCPagamento(requisicao.Pagamento, item.numero);
                                mensagens.Add(string.Format("Requisição compra número {0} gerada para a proposta {1}", item.numero, requisicao.Pagamento.CodigoProposta));

                                var msg = string.Format("Requisição compra número {0} gerada para a proposta {1}", item.numero, requisicao.Pagamento.CodigoProposta);
                                importTask.AppendLog(msg);
                            }
                            else
                            {
                                var erro = string.Format("{0}. referente a proposta {1}", item.status, requisicao.Pagamento.CodigoProposta);
                                LogError(ref importTask, erro);

                                mensagens.Add(string.Format("{0}. referente a proposta {1}", item.status, requisicao.Pagamento.CodigoProposta));
                            }
                        }

                        importTask.SuccessCount++;

                        transaction.Commit();
                    }
                    catch (BusinessRuleException bre)
                    {
                        transaction.Rollback();
                        foreach (var erro in bre.Errors)
                        {
                            LogError(ref importTask, erro);
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                importTask.Error = ex.Message;
                importTask.End = DateTime.Now;
                ExceptionLogger.LogException(ex);
            }
            finally
            {
                string targetFileLog = string.Format("{0}{1}{2}-log.txt", diretorio, Path.DirectorySeparatorChar,
                    importTask.TaskId);
                importTask.AppendLog(String.Format("Persistindo log em {0}, com identificador {1}",
                    Environment.MachineName, importTask.TaskId));
                File.WriteAllText(targetFileLog, importTask.FullLog.ToString());

                if (_session != null && _session.IsOpen)
                {
                    _session.Close();
                }

            }

            importTask.End = DateTime.Now;

            return mensagens;
        }
                
        public ViewPagamento ConvertToPagamento(ViewRelatorioVendaUnificado venda)
        {
            var pagamento = new ViewPagamento();

            pagamento.CodigoProposta = venda.CodigoProposta;
            pagamento.DivisaoEmpreendimento = venda.DivisaoEmpreendimento;
            pagamento.ValorAPagar = venda.ValorAPagar;
            pagamento.CodigoFornecedor = venda.CodigoFornecedor;
            pagamento.CodigoEmpresa = venda.CodigoEmpresa;
            pagamento.CodigoProposta = venda.CodigoProposta;
            pagamento.CodigoFornecedor = venda.CodigoFornecedorSap;
            pagamento.IdProposta = venda.IdProposta;
            pagamento.IdPagamento = venda.IdPagamento;
            pagamento.IdEmpresaVenda = venda.IdEmpresaVenda;
            pagamento.TipoPagamento = venda.TipoPagamento;

            return pagamento;
        }

        public byte[] ExportarPagamentosUnificado(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var pagamentos = _viewRelatorioVendaUnificadoRepository.ListarRelatorioVendaUnificado(request, filtro).records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString())
                .WithHeader(GetHeaderUnificado());

            foreach (var proposta in pagamentos)
            {
                excel
                    .CreateCellValue(proposta.NomeEmpresaVenda).Width(20)
                    .CreateCellValue(proposta.CentralVenda).Width(20)
                    .CreateCellValue(proposta.NomeEmpreendimento).Width(20)
                    .CreateCellValue(proposta.CodigoProposta).Width(20)
                    .CreateCellValue(proposta.NomeCliente).Width(20)
                    .CreateCellValue(proposta.PassoAtual).Width(20)
                    .CreateDateTimeCell(proposta.DataVenda).Width(20)
                    .CreateCellValue(proposta.StatusRepasse).Width(20)
                    .CreateCellValue(proposta.EmConformidade ? "Sim" : "Não").Width(20)
                    .CreateCellValue(proposta.SituacaoNotaFiscal.AsString()).Width(20)
                    .CreateCellValue(RenderFaixa(proposta)).Width(20)
                    .CreateCellValue(RenderPorcentagem(proposta)).Width(20)
                    .CreateCellValue(RegraPagamento(proposta.RegraPagamento, proposta.TipoPagamento)).Width(20)
                    .CreateDateTimeCell(proposta.DataComissao).Width(20)
                    .CreateMoneyCell(Convert.ToDecimal((proposta.ValorVGV.HasValue ? proposta.ValorVGV.Value.ToString("F") : "0"))).Width(20)
                    .CreateMoneyCell(Convert.ToDecimal(RenderValorAPagar(proposta).ToString("F"))).Width(20)
                    .CreateCellValue(proposta.CodigoRegraComissao).Width(20)
                    .CreateCellValue(proposta.EmReversao ? "Sim" : "Não").Width(20)
                    .CreateCellValue(proposta.ReciboCompra).Width(20)
                    .CreateDateTimeCell(proposta.DataRequisicaoCompra).Width(20)
                    .CreateCellValue(proposta.PedidoSap).Width(20)
                    .CreateDateTimeCell(proposta.DataPedidoSap).Width(20)
                    .CreateCellValue(proposta.MIRO).Width(20)
                    .CreateCellValue(proposta.MIGO).Width(20)
                    .CreateCellValue(proposta.MIR7).Width(20)
                    .CreateCellValue(proposta.NotaFiscal).Width(20)
                    .CreateCellValue(proposta.ChamadoPagamento).Width(20)
                    .CreateDateTimeCell(proposta.DataPrevisaoPagamento).Width(20)
                    .CreateCellValue(proposta.Pago ? "Sim" : "Não").Width(20)
                    .CreateCellValue(proposta.EmReversao ? "Cancelado" : " ").Width(20)
                    .CreateCellValue(proposta.Faturado ? GlobalMessages.Sim : GlobalMessages.Nao);
            }
                       
            excel.Close();
            return excel.DownloadFile();
        }

        public string RenderFaixa(ViewRelatorioVendaUnificado full)
        {
            switch (full.Tipologia)
            {
                case Tipologia.PNE:
                    return "PNE";
                    
                case Tipologia.FaixaUmMeio:
                    return "F 1,5";
                    
                case Tipologia.FaixaDois:
                    return "F 2,0";                    
            }

            if (full.FlagFaixaUmMeio)
            {
                return "F 1,5";
            }

            return "F 2,0";
        }

        string RenderPorcentagem(ViewRelatorioVendaUnificado full)
        {
            decimal porcentagem = 0;

            switch (full.Tipologia)
            {
                case Tipologia.PNE:
                    porcentagem = full.ComissaoPagarPNE.Value;
                    break;
                case Tipologia.FaixaUmMeio:
                    porcentagem = full.ComissaoPagarUmMeio;
                    break;
                default:
                    porcentagem = full.ComissaoPagarDois;
                    break;
            }

            if (porcentagem>0)
            {
                var valor = "%";
                return valor = porcentagem.ToString().Replace(".", ",") + valor;
            }
            return porcentagem+"%";
        }

        private string[] GetHeaderUnificado()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Central,
                GlobalMessages.Empreendimento,
                GlobalMessages.Proposta,
                GlobalMessages.Cliente,
                GlobalMessages.Situacao,
                GlobalMessages.DataVenda,
                GlobalMessages.StatusRepasse,
                GlobalMessages.StatusConformidade,
                GlobalMessages.SituacaoNotaFiscal,
                GlobalMessages.FaixaPagamento,
                GlobalMessages.Percentual,
                GlobalMessages.PorcentagemComissao,
                GlobalMessages.DataAptidao,
                GlobalMessages.ValorSemPremiada,
                GlobalMessages.ValorPagar,
                GlobalMessages.CodigoRegra,
                GlobalMessages.EmReversao,
                GlobalMessages.RC,
                GlobalMessages.DataRC,
                GlobalMessages.NumeroPedido,
                GlobalMessages.DataPedidoSap,
                GlobalMessages.MIRO,
                GlobalMessages.MIGO,
                GlobalMessages.MIR7,
                GlobalMessages.NotaFiscal,
                GlobalMessages.ChamadoPgto,
                GlobalMessages.PrevisaoPgto,
                GlobalMessages.Pagamento,
                GlobalMessages.SituacaoPagamento,
                GlobalMessages.Faturado
            };
            return header.ToArray();
        }
        public decimal RenderValorAPagar(ViewRelatorioVendaUnificado full)
        {

            decimal valor = 0;

            switch (full.Tipologia)
            {
                case Tipologia.PNE:
                    valor = full.ValorVGV.Value * full.ComissaoPagarPNE.Value / 100;
                    break;
                case Tipologia.FaixaUmMeio:
                    valor = full.ValorVGV.Value * full.ComissaoPagarUmMeio / 100;
                    break;
                default:
                    valor = full.ValorVGV.Value * full.ComissaoPagarDois / 100;
                    break;
            }

            return valor;
        }
        #endregion
    }
}
