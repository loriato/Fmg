using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Maps
{
    public class CautelaMap : BaseClassMap<Cautela>
    {
        public CautelaMap()
        {
            Table("TBL_CAUTELAS");
            Id(reg => reg.Id).Column("ID_CAUTELA").GeneratedBy.Sequence("SEQ_CAUTELAS");
            Map(reg => reg.Nome).Column("DS_NOME").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Marca).Column("DS_MARCA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Total).Column("NR_TOTAL");
        }
    }
}
