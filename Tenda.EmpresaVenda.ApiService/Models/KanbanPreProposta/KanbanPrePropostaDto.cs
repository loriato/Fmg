using System.Collections.Generic;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class KanbanPrePropostaDto
    {
        public List<ColumnKanbanPrePropostaDto> ColumnsKanbanPrePropostaDto { get; set; }
        public KanbanPrePropostaDto()
        {
            ColumnsKanbanPrePropostaDto = new List<ColumnKanbanPrePropostaDto>();
        }
    }
}
