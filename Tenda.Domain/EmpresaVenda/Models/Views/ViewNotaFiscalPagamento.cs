using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewNotaFiscalPagamento : BaseEntity
    {
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdProposta { get; set; }
        public virtual long? IdArquivo { get; set; }
        public virtual long? IdNotaFiscalPagamento { get; set; }
        public virtual long IdPagamento { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual DateTime? DataComissao { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual bool Pago { get; set; }
        public virtual string PedidoSap { get; set; }
        public virtual DateTime? DataPedidoSap { get; set; }
        public virtual DateTime? DataPrevisaoPagamento { get; set; }
        public virtual string NotaFiscal { get; set; }
        public virtual string RegraPagamento { get; set; }
        public virtual SituacaoNotaFiscal SituacaoNotaFiscal { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string Motivo { get; set; }
        public virtual string Estado { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual string Regional { get; set; }
        public virtual decimal ValorAPagar { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual string NomeCliente { get; set; }

        public virtual string NomeEmpreendimento { get; set; }
        public virtual string DescricaoTorre { get; set; }
        public virtual string DescricaoUnidade { get; set; }
        public virtual decimal ValorVgv { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual TipoModalidadeComissao ModalidadeComissao { get; set; }
        public virtual Tipologia Tipologia { get; set; }
        public virtual DateTime? DataVenda { get; set; }
        public virtual decimal Percentual { get; set; }
        public virtual TipoSubtabelaComissao SubTabela { get; set; }
        public virtual bool EmReversao { get; set; }
        public virtual string PassoAtual { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual DateTime? DataFaturado { get; set; }
        public virtual string CnpjEmpresaVenda { get; set; }
        public virtual int RevisaoNF { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
