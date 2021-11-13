using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class CoordenadorSupervisorMap : BaseClassMap<CoordenadorSupervisor>
    {
        public CoordenadorSupervisorMap()
        {
            Table("CRZ_COORDENADOR_SUPERVISOR");

            Id(reg => reg.Id).Column("ID_CRZ_COORDENADOR_SUPERVISOR").GeneratedBy.Sequence("SEQ_COORDENADOR_SUPERVISOR");
            References(reg => reg.Supervisor).Column("ID_SUPERVISOR").ForeignKey("FK_COORDENADOR_SUPERVISOR_X_USUARIO_01");
            References(reg => reg.Coordenador).Column("ID_COORDENADOR").ForeignKey("FK_COORDENADOR_SUPERVISOR_X_USUARIO_02");
        }
    }
}
