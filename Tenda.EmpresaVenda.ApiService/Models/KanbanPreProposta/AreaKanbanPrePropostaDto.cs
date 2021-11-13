using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class AreaKanbanPrePropostaDto: ItemKanbanPrePropostaDto
    {
        public List<CardKanbanPrePropostaDto> CardsKanban { get; set; }

        public AreaKanbanPrePropostaDto()
        {
            CardsKanban = new List<CardKanbanPrePropostaDto>();
        }

        public AreaKanbanPreProposta ToModel()
        {
            var areaKanban = new AreaKanbanPreProposta();

            areaKanban.Id = Id;
            areaKanban.Descricao = Descricao;
            areaKanban.Cor = Cor;
            areaKanban.Ativo = Ativo;
            return areaKanban;
        }

        public void FromDomain(AreaKanbanPreProposta areaKanbanPreProposta)
        {
            Id = areaKanbanPreProposta.Id;
            Descricao = areaKanbanPreProposta.Descricao;
            Cor = areaKanbanPreProposta.Cor;
            Ativo = areaKanbanPreProposta.Ativo;
        }
    }
}
