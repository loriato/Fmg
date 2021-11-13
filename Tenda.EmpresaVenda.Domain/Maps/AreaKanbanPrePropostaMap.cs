using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AreaKanbanPrePropostaMap : BaseClassMap<AreaKanbanPreProposta>
    {
        public AreaKanbanPrePropostaMap()
        {
            Table("TBL_AREA_KANBAN_PRE_PROPOSTA");

            Id(reg => reg.Id).Column("ID_AREA_KANBAN_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_AREA_KANBAN_PRE_PROPOSTA");

            Map(reg => reg.Descricao).Column("DS_AREA_KANBAN_PRE_PROPOSTA").Not.Nullable();
            Map(reg => reg.Cor).Column("DS_COR").Not.Nullable();
            Map(reg => reg.Ativo).Column("TP_SITUACAO").Not.Nullable();
        }
    }
}
