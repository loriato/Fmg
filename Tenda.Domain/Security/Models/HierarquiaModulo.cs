using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class HierarquiaModulo : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual HierarquiaModulo Pai { get; set; }
        public virtual Sistema Sistema { get; set; }
        public virtual int Ordem { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
