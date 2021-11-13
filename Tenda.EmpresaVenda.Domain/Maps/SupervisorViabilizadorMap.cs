using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class SupervisorViabilizadorMap : BaseClassMap<SupervisorViabilizador>
    {
        public SupervisorViabilizadorMap()
        {
            Table("CRZ_SUPERVISOR_VIABILIZADOR");

            Id(reg => reg.Id).Column("ID_CRZ_SUPERVISOR_VIABILIZADOR").GeneratedBy.Sequence("SEQ_SUPERVISOR_VIABILIZADOR");
            References(reg => reg.Supervisor).Column("ID_SUPERVISOR").ForeignKey("FK_SUPERVISOR_VIABILIZADOR_X_USUARIO_01");
            References(reg => reg.Viabilizador).Column("ID_VIABILIZADOR").ForeignKey("FK_SUPERVISOR_VIABILIZADOR_X_USUARIO_02");
        }
    }
}
