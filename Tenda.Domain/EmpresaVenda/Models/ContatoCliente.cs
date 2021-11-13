using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ContatoCliente : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual TipoContato Tipo { get; set; }
        public virtual string Contato { get; set; }
        public virtual bool Preferencial { get; set; }

        public override string ChaveCandidata()
        {
            return Contato;
        }
    }
}
