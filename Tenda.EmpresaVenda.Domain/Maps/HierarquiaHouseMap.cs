using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class HierarquiaHouseMap:BaseClassMap<HierarquiaHouse>
    {
        public HierarquiaHouseMap()
        {
            Table("TBL_HIERARQUIA_HOUSE");

            Id(reg => reg.Id).Column("ID_HIERARQUIA_HOUSE").GeneratedBy.Sequence("SEQ_HIERARQUIA_HOUSE");

            Map(reg => reg.Inicio).Column("DT_INICIO").Not.Nullable();
            Map(reg => reg.Fim).Column("DT_FIM").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoHierarquiaHouse>>().Not.Nullable();

            References(reg => reg.AgenteVenda).Column("ID_AGENTE_VENDA").ForeignKey("FK_HIERARQUIA_HOUSE_X_USUARIO_PORTAL_01");
            References(reg => reg.Supervisor).Column("ID_SUPERVISOR").ForeignKey("FK_HIERARQUIA_HOUSE_X_USUARIO_PORTAL_02");
            References(reg => reg.Coordenador).Column("ID_COORDENADOR").ForeignKey("FK_HIERARQUIA_HOUSE_X_USUARIO_PORTAL_03");
            References(reg => reg.House).Column("ID_HOUSE").ForeignKey("FK_HIERARQUIA_HOUSE_X_EMPRESA_VENDA_01");
        }
    }
}
