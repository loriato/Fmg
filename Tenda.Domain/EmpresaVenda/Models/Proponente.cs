using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Proponente : BaseEntity
    {
        public virtual PreProposta PreProposta { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual bool Titular { get; set; }
        public virtual int Participacao { get; set; }
        public virtual TipoReceitaFederal? ReceitaFederal { get; set; }
        public virtual decimal OrgaoProtecaoCredito { get; set; }
        public virtual decimal RendaApurada { get; set; }
        public virtual decimal RendaFormal { get; set; }
        public virtual decimal RendaInformal { get; set; }
        public virtual decimal FgtsApurado { get; set; }
        public virtual long IdSuat { get; set; }
        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
