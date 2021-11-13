using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Profissao : BaseEntity
    {
        public virtual string Nome { get; set; }

        public virtual string IdSap { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
