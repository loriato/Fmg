using Europa.Data;
using Tenda.Domain.Fmg.Models.Views;

namespace Europa.Fmg.Domain.Maps
{
    public class ViewUsuarioPedidoCautelaMap : BaseClassMap<ViewUsuarioPedidoCautela>
    {
        public ViewUsuarioPedidoCautelaMap()
        {
            Table("VW_USUARIO_PEDIDOS_CAUTELA");
            ReadOnly();
            SchemaAction.None();

            Id(x => x.Id).Column("ID_PEDIDO_CAUTELA");
            Map(x => x.IdUsuario).Column("ID_USUARIO");
            Map(x => x.IdCautela).Column("ID_CAUTELA");
            Map(x => x.NomeUsuario).Column("NM_USUARIO");
            Map(x => x.Cautela).Column("NM_CAUTELA");
            Map(x => x.DataPedido).Column("DT_PEDIDO");
            Map(x => x.DataDevolucao).Column("DT_DEVOLUCAO");
            Map(x => x.Quantidade).Column("NR_QUANTIDADE");
        }
    }
}
