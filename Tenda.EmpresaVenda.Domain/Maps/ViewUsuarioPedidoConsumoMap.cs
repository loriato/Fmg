using Europa.Data;
using Tenda.Domain.Fmg.Models.Views;

namespace Europa.Fmg.Domain.Maps
{
    public class ViewUsuarioPedidoConsumoMap : BaseClassMap<ViewUsuarioPedidoConsumo>
    {
        public ViewUsuarioPedidoConsumoMap()
        {
            Table("VW_USUARIO_PEDIDOS_CONSUMO");
            ReadOnly();
            SchemaAction.None();

            Id(x => x.Id).Column("ID_PEDIDO_CONSUMO");
            Map(x => x.IdUsuario).Column("ID_USUARIO");
            Map(x => x.IdConsumo).Column("ID_CONSUMO");
            Map(x => x.NomeUsuario).Column("NM_USUARIO");
            Map(x => x.Consumo).Column("NM_CONSUMO");
            Map(x => x.DataPedido).Column("DT_PEDIDO");
            Map(x => x.Quantidade).Column("NR_QUANTIDADE");
        }
    }
}
