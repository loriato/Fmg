using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Fmg.Models
{
    public class PedidoCautela : BaseEntity
    {
        public virtual Cautela Cautela { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual DateTime Pedido { get; set; }
        public virtual DateTime? Devolucao { get; set; }
        public virtual long Quantidade { get; set; }
        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
