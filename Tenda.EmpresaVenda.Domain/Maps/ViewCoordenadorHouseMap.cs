using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCoordenadorHouseMap:BaseClassMap<ViewCoordenadorHouse>
    {
        public ViewCoordenadorHouseMap()
        {
            Table("VW_COORDENADOR_HOUSE");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_USUARIO");

            Map(reg => reg.IdCoordenadorHouse).Column("ID_COORDENADOR_HOUSE");
            Map(reg => reg.NomeCoordenadorHouse).Column("NM_COORDENADOR_HOUSE");
            Map(reg => reg.LoginCoordenadorHouse).Column("DS_LOGIN_COORDENADOR_HOUSE");
        }
    }
}
