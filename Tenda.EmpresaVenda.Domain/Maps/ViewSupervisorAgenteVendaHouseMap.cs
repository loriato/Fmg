using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewSupervisorAgenteVendaHouseMap:BaseClassMap<ViewSupervisorAgenteVendaHouse>
    {
        public ViewSupervisorAgenteVendaHouseMap()
        {
            Table("VW_SUPERVISOR_AGENTE_VENDA_HOUSE");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_SUPERVISOR_AGENTE_VENDA_HOUSE");

            Map(reg => reg.IdUsuarioAgenteVenda).Column("ID_USUARIO_AGENTE_VENDA");
            Map(reg => reg.NomeUsuarioAgenteVenda).Column("NM_USUARIO_AGENTE_VENDA");
            Map(reg => reg.LoginUsuarioAgenteVenda).Column("DS_LOGIN_USUARIO_AGENTE_VENDA");
            Map(reg => reg.EmailUsuarioAgenteVenda).Column("DS_EMAIL_AGENTE_HOUSE");
            Map(reg => reg.IdAgenteVenda).Column("ID_AGENTE_VENDA");

            Map(reg => reg.IdSupervisorHouse).Column("ID_USUARIO_SUPERVISOR");
            Map(reg => reg.NomeSupervisorHouse).Column("NM_USUARIO_SUPERVISOR");
            Map(reg => reg.LoginSupervisorHouse).Column("DS_LOGIN_USUARIO_SUPERVISOR");

            Map(reg => reg.IdCoordenadorHouse).Column("ID_COORDENADOR_HOUSE");

            Map(reg => reg.IdHouse).Column("ID_HOUSE");
            Map(reg => reg.NomeHouse).Column("NM_HOUSE");
            Map(reg => reg.RegionalHouse).Column("DS_REGIONAL_HOUSE");

            Map(reg => reg.IdSupervisorAgenteVendaHouse).Column("ID_SUPERVISOR_AGENTE_VENDA_HOUSE");
            Map(reg => reg.IdAgenteVendaHouse).Column("ID_AGENTE_VENDA_HOUSE");

            Map(reg => reg.Ativo).Column("FL_ATIVO");
        }
    }
}
