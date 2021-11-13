using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCoordenadorSupervisorHouseMap:BaseClassMap<ViewCoordenadorSupervisorHouse>
    {
        public ViewCoordenadorSupervisorHouseMap()
        {
            Table("VW_COORDENADOR_SUPERVISOR_HOUSE");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_COORDENADOR_SUPERVISOR_HOUSE");

            Map(reg => reg.IdSupervisorHouse).Column("ID_USUARIO_SUPERVISOR");
            Map(reg => reg.NomeSupervisorHouse).Column("NM_USUARIO_SUPERVISOR");
            Map(reg => reg.LoginSupervisorHouse).Column("DS_LOGIN_SUPERVISOR");
            Map(reg => reg.IdCoordenadorHouse).Column("ID_USUARIO_COORDENADOR");
            Map(reg => reg.IdCoordenadorSupervisorHouse).Column("ID_COORDENADOR_SUPERVISOR_HOUSE");
            Map(reg => reg.NomeCoordenadorHouse).Column("NM_USUARIO_COORDENADOR");
            Map(reg => reg.LoginCoordenadorHouse).Column("DS_LOGIN_COORDENADOR");
            Map(reg => reg.Ativo).Column("FL_ATIVO");
        }
    }
}
