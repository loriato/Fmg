using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class CardKanbanPrePropostaMap:BaseClassMap<CardKanbanPreProposta>
    {
        public CardKanbanPrePropostaMap()
        {
            Table("TBL_CARD_KANBAN_PRE_PROPOSTA");

            Id(reg => reg.Id).Column("ID_CARD_KANBAN_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_CARD_KANBAN_PRE_PROPOSTA");

            Map(reg => reg.Descricao).Column("DS_CARD_KANBAN_PRE_PROPOSTA").Not.Nullable();
            Map(reg => reg.Cor).Column("DS_COR").Not.Nullable();

            References(reg => reg.AreaKanbanPreProposta).Column("ID_AREA_KANBAN_PRE_PROPOSTA").ForeignKey("FK_AREA_KANBAN_PRE_PROPOSTA_X_CARD_KANBAN_PRE_PROPOSTA_01").Not.Nullable() ;
        }
    }
}
