using Europa.Data.Model;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class TipoDocumentoAvalista : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual bool Obrigatorio { get; set; }

        public virtual int Ordem { get; set; }
        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
