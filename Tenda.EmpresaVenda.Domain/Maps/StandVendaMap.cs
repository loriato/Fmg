using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class StandVendaMap : BaseClassMap<StandVenda>
    {
        public StandVendaMap()
        {
            Table("TBL_STAND_VENDA");

            Id(reg => reg.Id).Column("ID_STAND_VENDA").GeneratedBy.Sequence("SEQ_STANDS_VENDAS");

            Map(reg => reg.Nome).Column("NM_STAND_VENDA").Not.Nullable();
            Map(reg => reg.Regional).Column("DS_REGIONAL").Not.Nullable();
            Map(reg => reg.Estado).Column("DS_ESTADO").Not.Nullable();
            Map(reg => reg.IdRegional).Column("ID_REGIONAL").Nullable();

        }
    }
}
