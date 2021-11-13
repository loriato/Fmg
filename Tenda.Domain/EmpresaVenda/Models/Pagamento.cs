using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Pagamento : BaseEntity
    {
        public virtual PropostaSuat Proposta { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual decimal? ValorPagamento { get; set; }
        public virtual string PedidoSap { get; set; }
        public virtual DateTime? DataPedidoSap { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual string ReciboCompra { get; set; }
        public virtual string ChamadoPedido { get; set; }
        public virtual string MIR7 { get; set; }
        public virtual string NotaFiscal { get; set; }
        public virtual string ChamadoPagamento { get; set; }
        public virtual DateTime? DataPrevisaoPagamento { get; set; }
        public virtual bool Pago { get; set; }
        public virtual NotaFiscalPagamento NotaFiscalPagamento { get; set; }
        public virtual StatusIntegracaoSap StatusIntegracaoSap { get; set; }
        public virtual DateTime? DataRequisicaoCompra { get; set; }
        public virtual string MIRO { get; set; }
        public virtual string MIGO { get; set; }
        public override string ChaveCandidata()
        {
            return PedidoSap;
        }

        public override string ToString()
        {
            return TipoPagamento.ToString() + " | " + PedidoSap + " | " + Situacao.ToString() + " | " + ReciboCompra + " | " + Pago + " | " + StatusIntegracaoSap.ToString();
        }
    }
}
