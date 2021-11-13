using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewGrupoCCAUsuarioMap:BaseClassMap<ViewGrupoCCAUsuario>
    {
        public ViewGrupoCCAUsuarioMap()
        {
            Table("VW_GRUPO_CCA_USUARIO");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_GRUPO_CCA_USUARIO");

            Map(reg => reg.IdUsuario).Column("ID_USUARIO");
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO");
            Map(reg => reg.IdGrupoCCA).Column("ID_GRUPO_CCA");
            Map(reg => reg.IdUsuarioGrupoCCA).Column("ID_USUARIO_GRUPO_CCA");
            Map(reg => reg.Ativo).Column("FL_ATIVO");
        }
    }
}
