using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ItemRegraComissao : BaseEntity
    {
        public virtual RegraComissao RegraComisao { get; set; }
        public virtual Empreendimento Empreendimento { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual double FaixaUmMeio { get; set; }
        public virtual double FaixaDois { get; set; }
        public virtual double ValorKitCompleto { get; set; }
        public virtual double ValorConformidade { get; set; }
        public virtual double ValorRepasse { get; set; }
        public virtual TipoModalidadeComissao TipoModalidadeComissao { get; set; }

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

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
