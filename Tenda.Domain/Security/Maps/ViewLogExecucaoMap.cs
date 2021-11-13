using Europa.Data;
using Tenda.Domain.Security.Views;


namespace Tenda.Domain.Security.Maps
{
    public class ViewLogExecucaoMap : BaseClassMap<ViewLogExecucao>
    {
        public ViewLogExecucaoMap()
        {
            Table("VW_LOG_EXECUCAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_EXECUCAO");
            Map(reg => reg.IdQuartzConfiguration).Column("ID_QUARTZ_CONFIGURATION");
            Map(reg => reg.DataInicioExecucao).Column("DT_INICIO_EXECUCAO");
            Map(reg => reg.DataTerminoExecucao).Column("DT_FIM_EXECUCAO");
            Map(reg => reg.Log).Column("DS_LOG_EXECUCAO");
        }
    }
}
