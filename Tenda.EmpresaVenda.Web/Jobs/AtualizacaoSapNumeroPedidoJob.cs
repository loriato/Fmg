using Europa.Commons;
using Europa.Extensions;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Zeus;
using Tenda.EmpresaVenda.Domain.Repository;


namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class AtualizacaoSapNumeroPedidoJob : BaseJob
    {
        public PagamentoRepository _pagamentoRepository { get; set; }
        public NumeroPedidoSapRepository _numeroPedidoSapRepository { get; set; }
        protected override void Init()
        {
            _pagamentoRepository = new PagamentoRepository();
            _pagamentoRepository._session = _session;
            _numeroPedidoSapRepository = new NumeroPedidoSapRepository();
            _numeroPedidoSapRepository._session = _session;
        }

        public override void Process()
        {
            ITransaction transaction = null;
            try
            {
                var rcs = _pagamentoRepository.BuscarRCsSemNumeroPedido();

                WriteLog(TipoLog.Informacao,
                    $"Buscando novos numeros do pedido para os pagamento");

                if (rcs.HasValue())
                {
                    WriteLog(TipoLog.Informacao,
                   String.Format("Quantidade de RCs sem numero pedido: {0}", rcs.Count()));
                    var results = ZeusService.BuscarPedido(rcs);

                    transaction = _session.BeginTransaction();
                    foreach (var item in results)
                    {
                        var pagamento = _pagamentoRepository.BuscarPorRC(item.NumeroRequisicaoCompra);

                        if (pagamento.HasValue())
                        {
                            pagamento.PedidoSap = item.NumeroDocumentoCompra;
                            pagamento.DataPedidoSap = DateTime.Now;
                            pagamento.StatusIntegracaoSap = StatusIntegracaoSap.PedidoGerado;

                            pagamento.AtualizadoEm = DateTime.Now;
                            pagamento.AtualizadoPor = ProjectProperties.IdUsuarioSistema;

                            _pagamentoRepository.Save(pagamento);

                            WriteLog(TipoLog.Informacao,
                            String.Format("Numero pedido {0} atualizado para  a RC {1}", pagamento.PedidoSap, pagamento.ReciboCompra));

                            var numeroPedidoSap = new NumeroPedidoSap
                            {
                                Mandante = item.Mandante,
                                CodigoLiberacaoDocumentoCompra = item.CodigoLiberacaoDocumentoCompra,
                                NumeroItemDocumentoCompra = item.NumeroItemDocumentoCompra,
                                NumeroDocumentoCompra = item.NumeroDocumentoCompra,
                                NumeroItemRequisicaoCompra = item.NumeroItemRequisicaoCompra,
                                NumeroRequisicaoCompra = item.NumeroRequisicaoCompra,
                                Data = item.Data,
                                Status = item.Status,
                                LinhaTexto = item.LinhaTexto
                            };
                            _numeroPedidoSapRepository.Save(numeroPedidoSap);
                        }
                    }
                    transaction.Commit();
                }

                WriteLog(TipoLog.Informacao,
                    "Execução do robô de atualização sap dos número do pedido dos pagamentos finalizada com sucesso.");
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                ExceptionLogger.LogException(e);
                WriteLog(TipoLog.Erro,
                    String.Format("Erro ao atualizar os número do pedido dos pagamentos: {0}.", e.Message));
            }
        }
    }
}