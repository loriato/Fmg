using Europa.Data.Model;
using System;

namespace Tenda.Domain.Fmg.Models.Views
{
    public class ViewUsuarioPedidoCautela : BaseEntity
    {
        public virtual long IdUsuario { get; set; }
        public virtual long IdCautela { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual string Cautela { get; set; }
        public virtual long? Quantidade { get; set; }
        public virtual DateTime? DataPedido { get; set; }
        public virtual DateTime? DataDevolucao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
