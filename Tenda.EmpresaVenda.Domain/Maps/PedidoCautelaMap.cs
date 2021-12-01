using Europa.Data;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Maps
{
    public class PedidoCautelaMap : BaseClassMap<PedidoCautela>
    {
        public PedidoCautelaMap()
        {
            Table("TBL_PEDIDO_CAUTELAS");
            Id(reg => reg.Id).Column("ID_PEDIDO_CAUTELA").GeneratedBy.Sequence("SEQ_PEDIDO_CAUTELAS");
            Map(reg => reg.Pedido).Column("DT_PEDIDO");
            Map(reg => reg.Devolucao).Column("DT_DEVOLUCAO").Nullable();
            Map(reg => reg.Quantidade).Column("NR_QUANTIDADE");



            References(x => x.Cautela).Column("ID_CAUTELA").ForeignKey("FK_CAUTELA_X_PEDIDO_CAUTELA_01").Nullable();
            References(x => x.Usuario).Column("ID_USUARIO").ForeignKey("FK_USUARIO_X_PEDIDO_CAUTELA_01").Nullable();
        }
    }
}
