using Europa.Data.Model;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class OrigemLead : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual UsuarioPortal Usuario { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
