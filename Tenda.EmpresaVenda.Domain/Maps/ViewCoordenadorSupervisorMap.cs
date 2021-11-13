using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCoordenadorSupervisorMap : BaseClassMap<ViewCoordenadorSupervisor>
    {
        public ViewCoordenadorSupervisorMap()
        {
            Table("VW_COORDENADOR_SUPERVISOR");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_COORDENADOR_SUPERVISOR");

            Map(reg => reg.IdSupervisor).Column("ID_SUPERVISOR");
            Map(reg => reg.NomeSupervisor).Column("NM_SUPERVISOR");

            Map(reg => reg.IdCoordenador).Column("ID_COORDENADOR");
            Map(reg => reg.NomeCoordenador).Column("NM_COORDENADOR");

            Map(reg => reg.IdCoordenadorSupervisor).Column("ID_CRZ_COORDENADOR_SUPERVISOR");

            Map(reg => reg.Ativo).Column("FL_ATIVO");
        }
    }
}
