using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewLojaPortalUsuarioMap : BaseClassMap<ViewLojaPortalUsuario>
    {
        public ViewLojaPortalUsuarioMap()
        {
            Table("VW_LOJAS_PORTAL_USUARIO");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_LOJA_PORTAL_USUARIO");

            Map(reg => reg.IdUsuario).Column("ID_USUARIO");
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO");
            Map(reg => reg.IdLojaPortal).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.Ativo).Column("FL_ATIVO");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.Email).Column("DS_EMAIL");
        }
    }
}
