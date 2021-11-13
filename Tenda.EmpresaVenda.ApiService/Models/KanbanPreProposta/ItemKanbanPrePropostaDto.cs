using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class ItemKanbanPrePropostaDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public string Cor { get; set; }
    }
}
