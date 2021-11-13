using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Familiar : BaseEntity
    {
        public virtual TipoFamiliaridade Familiaridade { get; set; }
        public virtual Cliente Cliente1 { get; set; }
        public virtual Cliente Cliente2 { get; set; }

        public override string ChaveCandidata()
        {
            return null;
        }
    }
}
