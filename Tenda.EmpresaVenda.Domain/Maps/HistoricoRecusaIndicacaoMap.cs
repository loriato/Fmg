using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class HistoricoRecusaIndicacaoMap : BaseClassMap<HistoricoRecusaIndicacao>
    {
        public HistoricoRecusaIndicacaoMap()
        {
            Table("TBL_HISTORICOS_RECUSAS_INDICACAO");
            Id(reg => reg.Id).Column("ID_HISTORICO_RECUSA_INDICACAO").GeneratedBy.Sequence("SEQ_HISTORICOS_RECUSAS_INDICACAO");

            Map(reg => reg.SituacaoPreProposta).Column("TP_SITUACAO_PRE_PROPOSTA").CustomType<EnumType<SituacaoProposta>>();
            Map(reg => reg.DataMomento).Column("DT_MOMENTO");
            References(reg => reg.PreProposta).Column("ID_PREPROPOSTA").ForeignKey("FK_HIST_RECUSAS_IND_X_PRE_PROPOSTA_01");
            References(reg => reg.Responsavel).Column("ID_RESPONSAVEL").ForeignKey("FK_HIST_RECUSAS_IND_X_RESPONSAVEL_01");

        }
    }
}
