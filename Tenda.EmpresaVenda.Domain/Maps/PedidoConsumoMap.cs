using Europa.Data;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Maps
{
    public class PedidoConsumoMap : BaseClassMap<PedidoConsumo>
    {
        public PedidoConsumoMap()
        {
            Table("TBL_PEDIDO_CONSUMOS");
            Id(reg => reg.Id).Column("ID_PEDIDO_CONSUMO").GeneratedBy.Sequence("SEQ_PEDIDO_CONSUMOS");
            Map(reg => reg.Pedido).Column("DT_PEDIDO");
            Map(reg => reg.Quantidade).Column("DT_QUANTIDADE");

            References(x => x.Consumo).Column("ID_CONSUMO").ForeignKey("FK_CONSUMO_X_PEDIDO_CONSUMO_01").Nullable();
            References(x => x.Usuario).Column("ID_USUARIO").ForeignKey("FK_USUARIO_X_PEDIDO_CONSUMO_01").Nullable();
        }
    }
}
