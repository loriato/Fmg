using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Fmg.Models
{
    public class PedidoConsumo : BaseEntity
    {
        public virtual Consumo Consumo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual DateTime Pedido { get; set; }
        public virtual long Quantidade { get; set; }


        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
