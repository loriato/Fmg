using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Maps
{
    public class ConsumoMap : BaseClassMap<Consumo>
    {
        public ConsumoMap()
        {
            Table("TBL_CONSUMOS");
            Id(reg => reg.Id).Column("ID_CONSUMO").GeneratedBy.Sequence("SEQ_CONSUMOS");
            Map(reg => reg.Nome).Column("NM_CONSUMO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Lote).Column("DS_LOTE").Length(DatabaseStandardDefinitions.FortyLength);
            Map(reg => reg.Total).Column("NR_TOTAL");
            Map(reg => reg.Validade).Column("DT_VALIDADE");
        }
    }
}
