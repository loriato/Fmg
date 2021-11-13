using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class TranslateStatusPrePropostaMap : BaseClassMap<StatusPreProposta>
    {
        public TranslateStatusPrePropostaMap()
        {
            Table("TBL_TRANSLATE_STATUS_PREPROPOSTA");
            Id(reg => reg.Id).Column("ID_TRANSLATE_STATUS_PREPROPOSTA").GeneratedBy.Sequence("SEQ_TRANSLATE_STATUS_PREPROPOSTA");
            Map(reg => reg.StatusPadrao).Column("DS_STATUS_PADRAO");
            Map(reg => reg.StatusPortalHouse).Column("DS_STATUS_PORTAL_HOUSE");
            Map(reg => reg.SituacaoProposta).Column("TP_SITUACAO_PROPOSTA").CustomType < EnumType<SituacaoProposta>>();
        }
    }
}
