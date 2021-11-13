using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class CoordenadorSupervisorHouseMap:BaseClassMap<CoordenadorSupervisorHouse>
    {
        public CoordenadorSupervisorHouseMap()
        {
            Table("TBL_COORDENADOR_SUPERVISOR_HOUSE");

            Id(reg => reg.Id).Column("ID_COORDENADOR_SUPERVISOR_HOUSE").GeneratedBy.Sequence("SEQ_COORDENADOR_SUPERVISOR_HOUSE");

            Map(reg => reg.Inicio).Column("DT_INICIO").Not.Nullable();
            Map(reg => reg.Fim).Column("DT_FIM").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoHierarquiaHouse>>().Not.Nullable();

            References(reg => reg.Coordenador).Column("ID_COORDENADOR").ForeignKey("FK_COORDENADOR_SUPERVISOR_HOUSE_X_USUARIO_PORTAL_01");
            References(reg => reg.Supervisor).Column("ID_SUPERVISOR").ForeignKey("FK_COORDENADOR_SUPERVISOR_HOUSE_X_USUARIO_PORTAL_02");
        }
    }
}
