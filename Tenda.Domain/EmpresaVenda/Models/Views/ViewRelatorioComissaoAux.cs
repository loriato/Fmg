using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewRelatorioComissaoAux : BaseEntity
    {
        public virtual string Regional { get; set; }
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
        public virtual bool Repasse { get; set; }
        public virtual bool Conformidade { get; set; }
        public virtual bool FluxoCaixa { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}