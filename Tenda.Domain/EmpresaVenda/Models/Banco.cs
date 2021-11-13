using Europa.Data.Model;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Banco : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual Situacao Situacao { get; set; }

        public override string ChaveCandidata()
        {
            return Codigo + " - " + Sigla + " - " + Nome;
        }
    }
}
