using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class StandVenda : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Regional { get; set; }
        public virtual string Estado { get; set; }

        public virtual long IdRegional { get; set; }
        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
