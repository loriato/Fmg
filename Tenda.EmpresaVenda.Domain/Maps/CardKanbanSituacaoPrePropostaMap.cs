using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class CardKanbanSituacaoPrePropostaMap : BaseClassMap<CardKanbanSituacaoPreProposta>
    {
        public CardKanbanSituacaoPrePropostaMap()
        {
            Table("TBL_CARD_KANBAN_TRANSLATE_STATUS_PRE_PROPOSTA");

            Id(reg => reg.Id).Column("ID_CARD_KANBAN_TRANSLATE_STATUS_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_CARD_KANBAN_TRANSLATE_STATUS_PRE_PROPOSTA");

            Map(reg => reg.Cor).Column("DS_COR").Not.Nullable();

            References(reg => reg.CardKanbanPreProposta).Column("ID_CARD_KANBAN_PRE_PROPOSTA").ForeignKey("FK_CARD_KANBAN_PRE_PROPOSTA_X_CARD_KANBAN_TRANSLATE_STATUS_PRE_PROPOSTA_01").Not.Nullable();
            References(reg => reg.StatusPreProposta).Column("ID_TRANSLATE_STATUS_PRE_PROPOSTA").ForeignKey("FK_TRANSLATE_STATUS_PRE_PROPOSTA_X_CARD_KANBAN_TRANSLATE_STATUS_PRE_PROPOSTA_01").Not.Nullable();
        }
    }
}
