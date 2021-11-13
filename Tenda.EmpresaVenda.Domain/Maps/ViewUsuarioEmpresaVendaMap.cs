using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewUsuarioEmpresaVendaMap:BaseClassMap<ViewUsuarioEmpresaVenda>
    {
        public ViewUsuarioEmpresaVendaMap()
        {
            Table("VW_USUARIO_EMPRESA_VENDA");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_USUARIO_EMPRESA_VENDA");

            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.IdUsuario).Column("ID_USUARIO");
        }
    }
}
