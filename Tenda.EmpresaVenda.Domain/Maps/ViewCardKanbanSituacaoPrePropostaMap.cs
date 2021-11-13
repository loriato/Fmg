using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCardKanbanSituacaoPrePropostaMap : BaseClassMap<ViewCardKanbanSituacaoPreProposta>
    {
        public ViewCardKanbanSituacaoPrePropostaMap()
        {
            Table("VW_CARD_KANBAN_SITUACAO_PRE_PROPOSTA");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_CARD_KANBAN_TRANSLATE_STATUS_PRE_PROPOSTA");

            Map(reg => reg.IdAreaKanbanPreProposta).Column("ID_AREA_KANBAN_PRE_PROPOSTA");

            Map(reg => reg.IdCardKanbanPreProposta).Column("ID_CARD_KANBAN_PRE_PROPOSTA");
            Map(reg => reg.DescricaoCardKanbanPreProposta).Column("DS_CARD_KANBAN_PRE_PROPOSTA");

            Map(reg => reg.IdStatusPreProposta).Column("ID_TRANSLATE_STATUS_PREPROPOSTA");
            Map(reg => reg.StatusPadrao).Column("DS_STATUS_PADRAO");
            Map(reg => reg.StatusPortalHouse).Column("DS_STATUS_PORTAL_HOUSE");
            Map(reg => reg.SituacaoProposta).Column("TP_SITUACAO_PROPOSTA").CustomType<EnumType<SituacaoProposta>>();
            Map(reg => reg.Cor).Column("DS_COR");
        }
    }
}
