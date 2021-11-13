using Europa.Data.Model;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class UsuarioGrupoCCA:BaseEntity
    {
        public virtual UsuarioPortal Usuario { get;set; }
        public virtual GrupoCCA GrupoCCA { get; set; }

        public override string ChaveCandidata()
        {
            return Usuario.Nome;
        }
    }
}
