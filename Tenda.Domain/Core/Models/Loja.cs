using Europa.Data.Model;
using System;

namespace Tenda.Domain.Core.Models
{
    public class Loja : BaseEntity
    {
        public virtual string Nome { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
