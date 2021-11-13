using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPagamento : BaseEntity
    {
        public virtual string NomeLoja { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual string PassoAtual { get; set; }
        public virtual DateTime? DataPassoAtual { get; set; }
        public virtual DateTime? DataVenda { get; set; }
        public virtual decimal Percentual { get; set; }
        public virtual string RegraPagamento { get; set; }
        public virtual DateTime? DataComissao { get; set; }
        public virtual decimal ValorSemPremiada { get; set; }
        public virtual decimal ValorAPagar { get; set; }
        public virtual bool EmReversao { get; set; }
        public virtual bool Pago { get; set; }
        public virtual string PedidoSap { get; set; }
        public virtual long IdPagamento { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual long IdProposta { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string CodigoRegraComissao { get; set; }
        public virtual bool FaixaUmMeio { get; set; }
        public virtual long IdRegraComissao { get; set; }
        public virtual string ReciboCompra { get; set; }
        public virtual string ChamadoPedido { get; set; }
        public virtual string MIR7 { get; set; }
        public virtual string NotaFiscal { get; set; }
        public virtual string ChamadoPagamento { get; set; }
        public virtual DateTime? DataPrevisaoPagamento { get; set; }
        public virtual Tipologia Tipologia { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Regional { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual bool NumeroGerado { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual SituacaoNotaFiscal SituacaoNotaFiscal { get; set; }
        public virtual bool StatusConformidade { get; set; }
        public virtual string StatusRepasse { get; set; }
        public virtual StatusIntegracaoSap? StatusIntegracaoSap { get; set; }
        public virtual DateTime? DataRequisicaoCompra { get; set; }
        public virtual DateTime? DataPedidoSap { get; set; }
        public virtual string DivisaoEmpreendimento { get; set; }
        public virtual string CodigoEmpresa { get; set; }
        public virtual string CodigoFornecedor { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual DateTime? DataFaturado { get; set; }
        public virtual string MIRO { get; set; }
        public virtual string MIGO { get; set; }
        public override string ChaveCandidata()
        {
            return CodigoProposta + " | " + NomeEmpresaVenda + " | " + this.TipoPagamento.ToString() + " | " + ReciboCompra + " | " + PedidoSap;
        }
    }
}
