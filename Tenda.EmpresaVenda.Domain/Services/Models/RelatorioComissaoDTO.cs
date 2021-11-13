using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class RelatorioComissaoDTO
    {
        public long? IdEmpreendimento { get; set; }
        public string CodigoPreProposta { get; set; }
        public string CodigoProposta { get; set; }
        public List<string> Estados { get; set; }
        public List<long> Regionais { get; set; }
        public string NomeFornecedor { get; set; }
        public string CodigoFornecedor { get; set; }
        public DateTime? DataVendaDe { get; set; }
        public DateTime? DataVendaAte { get; set; }
        public string NomeCliente { get; set; }
        public string StatusContrato { get; set; }
        public List<long> IdsEmpresaVenda { get; set; }
        public DateTime PeriodoDe { get; set; }
        public DateTime PeriodoAte { get; set; }
        public int Pago { get; set; }
        public long IdEmpresaVenda { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public StatusAdiantamentoPagamento AdiantamentoPagamento { get; set; }
        public StatusIntegracaoSap? StatusIntegracaoSap { get; set; }
        public List<long> PontosVenda { get; set; }
        public DateTime? DataRcPedidoSapDe { get; set; }
        public DateTime? DataRcPedidoSapAte { get; set; }
        public int Faturado { get; set; }
        public DateTime? DataFaturadoDe { get; set; }
        public DateTime? DataFaturadoAte { get; set; }
        public DateTime? DataPrevisaoPagamentoInicio { get; set; }
        public DateTime? DataPrevisaoPagamentoTermino { get; set; }
        public TipoEmpresaVenda TipoEmpresaVenda { get; set; }
    }
}
