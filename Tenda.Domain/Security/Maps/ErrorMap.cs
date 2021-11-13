using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    class ErrorMap : BaseClassMap<Error>
    {
        public ErrorMap()
        {
            Table("TBL_ERRORS");

            Id(reg => reg.Id).Column("ID_ERROR").GeneratedBy.Sequence("SEQ_ERRORS");
            Map(reg => reg.Message).Column("DS_MESSAGE").Length(DatabaseStandardDefinitions.TwoThousandLength);
            Map(reg => reg.Caller).Column("DS_CALLER").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.OriginClass).Column("DS_ORIGIN_CLASS").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Target).Column("DS_TARGET").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.CodeLine).Column("NR_CODE_LINE").Nullable();
            Map(reg => reg.Source).Column("DS_SOURCE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.Stacktrace).Column("DS_STACKTRACE").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType).Nullable();
            Map(reg => reg.Type).Column("DS_TYPE").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.ContextInfo).Column("DS_CONTEXT_INFO").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType).Nullable();
        }
    }
}
