using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class LogExecucaoMap : BaseClassMap<LogExecucao>
    {
        public LogExecucaoMap()
        {
            Table("TBL_LOGS_EXECUCAO");

            Id(reg => reg.Id).Column("ID_LOG_EXECUCAO").GeneratedBy.Sequence("SEQ_LOGS_EXECUCAO");
            Map(reg => reg.Log).Column("DS_LOG_EXECUCAO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.Tipo).Column("TP_LOG_EXECUCAO").CustomType<EnumType<TipoLog>>();
            References(reg => reg.Execucao).Column("ID_EXECUCAO")
                .ForeignKey("FK_LOG_EXECUCAO_X_EXECUCAO_01");
        }
    }
}
