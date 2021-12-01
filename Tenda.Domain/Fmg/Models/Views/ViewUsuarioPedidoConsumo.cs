using Europa.Data.Model;
using System;

namespace Tenda.Domain.Fmg.Models.Views
{
    public class ViewUsuarioPedidoConsumo : BaseEntity
    {
        public virtual long IdUsuario { get; set; }
        public virtual long IdConsumo { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual string Consumo { get; set; }
        public virtual long? Quantidade { get; set; }
        public virtual DateTime? DataPedido { get; set; }
        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
