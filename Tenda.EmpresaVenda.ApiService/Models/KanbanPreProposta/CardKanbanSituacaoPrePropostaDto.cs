using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta
{
    public class CardKanbanSituacaoPrePropostaDto
    {
        public long IdCardKanbanPreProposta { get; set; }
        public long IdStatusPreProposta { get; set; }
        public string Cor { get; set; }

        public CardKanbanSituacaoPreProposta ToModel()
        {
            var model = new CardKanbanSituacaoPreProposta();

            model.CardKanbanPreProposta = new CardKanbanPreProposta { Id = IdCardKanbanPreProposta };
            model.StatusPreProposta = new Tenda.Domain.EmpresaVenda.Models.StatusPreProposta { Id = IdStatusPreProposta };
            model.Cor = Cor;

            return model;
        }
    }
}
