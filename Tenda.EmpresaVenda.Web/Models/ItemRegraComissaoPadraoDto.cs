using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class ItemRegraComissaoPadraoDto
    {
        public long IdItemRegraComissaoPadrao { get; set; }
        public long IdEmpreendimento { get; set; }
        public long IdRegraComissaoPadrao { get; set; }
        public double FaixaUmMeio { get; set; }
        public double FaixaDois { get; set; }
        public double ValorKitCompleto { get; set; }
        public double ValorConformidade { get; set; }
        public double ValorRepasse { get; set; }
        //faixa 1.5
        public virtual double MenorValorNominalUmMeio { get; set; }
        public virtual double IgualValorNominalUmMeio { get; set; }
        public virtual double MaiorValorNominalUmMeio { get; set; }

        //Faixa 2
        public virtual double MenorValorNominalDois { get; set; }
        public virtual double IgualValorNominalDois { get; set; }
        public virtual double MaiorValorNominalDois { get; set; }

        //Faixa PNE
        public virtual double MenorValorNominalPNE { get; set; }
        public virtual double IgualValorNominalPNE { get; set; }
        public virtual double MaiorValorNominalPNE { get; set; }
        public virtual TipoModalidadeComissao Modalidade { get; set; }
    }
}