using Europa.Extensions;
using System.Collections.Generic;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class FiltroKanbanPrePropostaDto : FiltroConsultaPrePropostaDto
    {
        public long IdAreaKanbanPreProposta { get; set; }
        public long IdCardKanbanPreProposta { get; set; }
        public string Descricao { get; set; }     
    }
}
