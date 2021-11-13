using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ViewRelatorioComissao : BaseEntity
    {
        public virtual long IdRegional { get; set; }
        public virtual string Regional { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Divisao { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string CentralVenda { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual DateTime? DataSicaq { get; set; }
        public virtual string StatusContrato { get; set; }
        public virtual string StatusRepasse { get; set; }
        public virtual DateTime? DataRepasse { get; set; }
        public virtual string StatusConformidade { get; set; }
        public virtual DateTime? DataConformidade { get; set; }
        public virtual string RegraPagamento { get; set; }
        public virtual string CodigoFornecedor { get; set; }
        public virtual string NomeFornecedor { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string DescricaoTorre { get; set; }
        public virtual string DescricaoUnidade { get; set; }
        public virtual DateTime? DataVenda { get; set; }
        public virtual decimal? ValorVGV { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual string DescricaoTipologia { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual decimal FaixaUmMeio { get; set; }
        public virtual decimal FaixaDois { get; set; }
        public virtual long IdRegraComissaoEvs { get; set; }
        public virtual DateTime? DataRegistro { get; set; }
        public virtual string Observacao { get; set; }
        public virtual decimal ComissaoPagarUmMeio { get; set; }
        public virtual decimal ComissaoPagarDois { get; set; }
        public virtual string CodigoRegra { get; set; }
        public virtual DateTime? DataKitCompleto { get; set; }
        public virtual long IdRegraComissao { get; set; }
        public virtual string PassoAtual { get; set; }
        //public virtual decimal ValorAPagar { get; set; }
        public virtual bool EmReversao { get; set; }
        public virtual bool Pago { get; set; }
        //public virtual string PedidoSap { get; set; }
        //public virtual DateTime? DataPassoAtual { get; set; }
        public virtual bool FlagFaixaUmMeio { get; set; }
        public virtual string EmpresaDeVenda { get; set; }
        //public virtual long? IdPagamento { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual long IdProposta { get; set; }
        //public virtual long IdItemRegraComissao { get; set; }
        //public virtual DateTime? DataComissao { get; set; }
        public virtual DateTime? DataPagamento { get; set; }
        public virtual bool KitCompleto { get; set; }
        public virtual StatusAdiantamentoPagamento AdiantamentoPagamento { get; set; }
        public virtual Tipologia Tipologia { get; set; }
        public virtual decimal? Faixa { get; set; }
        public virtual TipoModalidadeComissao Modalidade { get; set; }
        public virtual decimal? ComissaoPagarPNE { get; set; }
        public virtual long IdPontoVenda { get; set; }
        public virtual string NomePontoVenda { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual DateTime? DataFaturado { get; set; }
        public virtual DateTime? DataSicaqPreproposta { get; set; }

        public virtual TipoEmpresaVenda TipoEmpresaVenda { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
