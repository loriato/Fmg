using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewStandVendaMap : BaseClassMap<ViewStandVenda>
    {
        public ViewStandVendaMap()
        {
            Table("VW_STAND_VENDA");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_STAND_VENDA");

            Map(reg => reg.Nome).Column("NM_STAND_VENDA");
            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.NovoRegional).Column("NM_REGIONAL").Nullable();
            Map(reg => reg.IdRegional).Column("ID_REGIONAL").Nullable();

        }
    }
}
