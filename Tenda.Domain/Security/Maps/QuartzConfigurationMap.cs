using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class QuartzConfigurationMap : BaseClassMap<QuartzConfiguration>
    {
        public QuartzConfigurationMap()
        {
            Table("TBL_QUARTZ_CONFIGURATION");

            Id(x => x.Id).Column("ID_QUARTZ_CONFIGURATION").Not.Nullable().GeneratedBy.Sequence("SEQ_QUARTZ_CONFIGURATION");

            Map(x => x.Nome).Column("NM_QUARTZ_CONFIGURATION").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.IniciarAutomaticamente).Column("DS_INICIAR_AUTOMATICAMENTE");
            Map(x => x.ForcarDesligamento).Column("FL_FORCAR_DESLIGAMENTO");
            Map(x => x.Cron).Column("DS_CRON").Length(DatabaseStandardDefinitions.FiftyLength);
            Map(x => x.CaminhoCompleto).Column("DS_CAMINHO_COMPLETO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(x => x.ServidorExecucao).Column("DS_SERVIDOR_EXECUCAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(x => x.AplicacaoExecucao).Column("DS_APLICACAO_EXECUCAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(x => x.SiteExecucao).Column("DS_SITE_EXECUCAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(x => x.Observacoes).Column("DS_OBSERVACOES").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
        }
    }
}
