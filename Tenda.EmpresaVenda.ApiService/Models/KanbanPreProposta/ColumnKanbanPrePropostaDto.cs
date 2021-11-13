using System.Collections.Generic;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class ColumnKanbanPrePropostaDto
    {
        public AreaKanbanPrePropostaDto AreaKanbanPrePropostaDto { get; set; }
        public List<CardKanbanPrePropostaDto> CardsKanbanPrePropostaDto { get; set; }

        public ColumnKanbanPrePropostaDto()
        {
            AreaKanbanPrePropostaDto = new AreaKanbanPrePropostaDto();
            CardsKanbanPrePropostaDto = new List<CardKanbanPrePropostaDto>();
        }
    }
}
