using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class GrupoCCA:BaseEntity
    {
        public virtual string Descricao { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
