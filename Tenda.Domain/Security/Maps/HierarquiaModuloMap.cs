using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class HierarquiaModuloMap : BaseClassMap<HierarquiaModulo>

    {
        public HierarquiaModuloMap()
        {
            Table("TBL_HIERARQUIAS_MODULO");

            Id(reg => reg.Id).Column("ID_HIERARQUIA_MODULO").GeneratedBy.Sequence("SEQ_HIERARQUIAS_MODULO");
            Map(reg => reg.Nome).Column("NM_HIERARQUIA").Not.Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").Not.Nullable().CustomType<Situacao>();
            Map(reg => reg.Ordem).Column("NR_ORDEM").Nullable();
            References(reg => reg.Sistema).ForeignKey("FK_HIERA_MODU_X_SIST_01").Column("ID_SISTEMA");
            References(reg => reg.Pai).ForeignKey("FK_HIERA_MODU_X_HIERA_MODU_01").Column("ID_MODULO_PAI").Nullable();
        }
    }
}
