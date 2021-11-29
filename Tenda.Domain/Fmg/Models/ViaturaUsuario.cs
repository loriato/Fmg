using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Fmg.Models
{
    public class ViaturaUsuario : BaseEntity
    {
        public virtual Usuario Usuario { get; set; }
        public virtual Viatura Viatura { get; set; }
        public virtual DateTime Pedido { get; set; }
        public virtual DateTime? Entrega { get; set; }
        public virtual long QuilometragemAntigo { get; set; }
        public virtual long? QuilometragemNovo { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
