using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class CardKanbanPrePropostaDto : ItemKanbanPrePropostaDto
    {
        public long IdAreaKanban { get; set; }
        public long Quantidade { get; set; }
        public List<ViewCardKanbanSituacaoPreProposta> Situacoes { get; set; }

        public CardKanbanPrePropostaDto()
        {
            Situacoes = new List<ViewCardKanbanSituacaoPreProposta>();
        }

        public CardKanbanPreProposta ToModel()
        {
            var cardKanban = new CardKanbanPreProposta();

            cardKanban.Id = Id;
            cardKanban.Descricao = Descricao;
            cardKanban.Cor = Cor;
            cardKanban.AreaKanbanPreProposta = new AreaKanbanPreProposta { Id = IdAreaKanban };

            return cardKanban;
        }

        public void FromDomain(CardKanbanPreProposta cardKanbanPreProposta)
        {
            Id = cardKanbanPreProposta.Id;
            Descricao = cardKanbanPreProposta.Descricao;
            Cor = cardKanbanPreProposta.Cor;
            IdAreaKanban = cardKanbanPreProposta.AreaKanbanPreProposta.Id;
        }
    }
}
