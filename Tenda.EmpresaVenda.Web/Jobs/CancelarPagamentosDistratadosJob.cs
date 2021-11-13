using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.ApiService.Models.Financeiro;
using Tenda.EmpresaVenda.ApiService.Models.Midas;
using Tenda.EmpresaVenda.Domain.Integration.Zeus;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class CancelarPagamentosDistratadosJob : BaseJob
    {
        public ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository { get; set; }
        public NumeroPedidoSapRepository _numeroPedidoSapRepository { get; set; }
        public ZeusApiService _zeusApiService { get; set; }
        public PagamentoRepository _pagamentoRepository { get; set; }
        public PagamentoService _pagamentoService { get; set; }
        protected override void Init()
        {
            _viewRelatorioVendaUnificadoRepository = new ViewRelatorioVendaUnificadoRepository();
            _viewRelatorioVendaUnificadoRepository._session = _session;

            _pagamentoRepository = new PagamentoRepository();
            _pagamentoRepository._session = _session;

            _numeroPedidoSapRepository = new NumeroPedidoSapRepository();
            _numeroPedidoSapRepository._session = _session;

            _viewRelatorioVendaUnificadoRepository = new ViewRelatorioVendaUnificadoRepository();
            _viewRelatorioVendaUnificadoRepository._session = _session;

            var perBaseUrl = new PerBaseUrlFlurlClientFactory();
            _zeusApiService = new ZeusApiService(perBaseUrl);

            _pagamentoService = new PagamentoService();
            _pagamentoService._pagamentoRepository = _pagamentoRepository;
            _pagamentoService._viewRelatorioVendaUnificadoRepository = _viewRelatorioVendaUnificadoRepository;
            _pagamentoService._numeroPedidoSapRepository = _numeroPedidoSapRepository;
            _pagamentoService._zeusApiService = _zeusApiService;
            _pagamentoService._session = _session;

        }

        public override void Process()
        {
            var PedidosSap = _viewRelatorioVendaUnificadoRepository.Queryable()
                .Where(x => x.EmReversao == true || x.PassoAtual == "Prop. Cancelada")
                .Where(x => x.PedidoSap != null).ToList();

            


            if (PedidosSap.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Não há pagamentos distratados pra serem cancelados.");
                return;
            }

            var total = PedidosSap.Count();

            WriteLog(TipoLog.Informacao, string.Format("Total de pagamentos em reverção: {0}", total));
            
            foreach (var pagamento in PedidosSap)
            {
                var transaction = _session.BeginTransaction();
                WriteLog(TipoLog.Informacao, string.Format("Processando Pagamento: {0}", pagamento.PedidoSap));
                bool sucesso = _pagamentoService.RemoverPagamento(pagamento.IdPagamento);
                if (sucesso)
                {
                    WriteLog(TipoLog.Informacao, string.Format("Pagamento: {0} Removido com sucesso", pagamento.PedidoSap));
                    transaction.Commit();
                }
                else
                {
                    WriteLog(TipoLog.Erro, string.Format("Pagamento: {0} não foi encontrado no SAP", pagamento.PedidoSap));
                    transaction.Rollback();
                }
            }
        }
    }
}