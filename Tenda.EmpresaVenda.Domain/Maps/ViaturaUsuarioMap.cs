using Europa.Data;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Maps
{
    public class ViaturaUsuarioMap : BaseClassMap<ViaturaUsuario>
    {
        public ViaturaUsuarioMap()
        {
            Table("TBL_VIATURAS_USUARIOS");
            Id(reg => reg.Id).Column("ID_VIATURA_USUARIO").GeneratedBy.Sequence("SEQ_VIATURAS_USUARIOS");
            Map(reg => reg.Pedido).Column("DT_PEDIDO");
            Map(reg => reg.Entrega).Column("DT_ENTREGA").Nullable();
            Map(reg => reg.QuilometragemAntigo).Column("NR_QUILOMETRAGEM_ANTIGO");
            Map(reg => reg.QuilometragemNovo).Column("NR_QUILOMETRAGEM_NOVO").Nullable();


            References(x => x.Viatura).Column("ID_VIATURA").ForeignKey("FK_VIATURA_X_VIATURA_USUARIO_01").Nullable();
            References(x => x.Usuario).Column("ID_USUARIO").ForeignKey("FK_USUARIO_X_VIATURA_USUARIO_01").Nullable();
        }
    }
}
