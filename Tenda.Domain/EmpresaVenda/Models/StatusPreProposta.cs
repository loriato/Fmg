using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class StatusPreProposta : BaseEntity
    {
        public virtual SituacaoProposta SituacaoProposta { get; set; }
        public virtual string StatusPadrao { get; set; }
        public virtual string StatusPortalHouse { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
