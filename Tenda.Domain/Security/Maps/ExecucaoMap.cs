using Europa.Data;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class ExecucaoMap : BaseClassMap<Execucao>
    {
        public ExecucaoMap()
        {
            Table("TBL_EXECUCOES");

            Id(reg => reg.Id).Column("ID_EXECUCAO").GeneratedBy.Sequence("SEQ_EXECUCOES");
            Map(reg => reg.DataInicioExecucao).Column("DT_INICIO_EXECUCAO");
            Map(reg => reg.DataFimExecucao).Column("DT_FIM_EXECUCAO");
            References(reg => reg.Quartz).Column("ID_QUARTZ_CONFIGURATION")
                .ForeignKey("FK_EXECUCAO_X_QUARTZ_CONF_01");
        }
    }
}
