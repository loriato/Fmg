using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class CardKanbanSituacaoPrePropostaValidator:AbstractValidator<CardKanbanSituacaoPreProposta>
    {
        private CardKanbanSituacaoPrePropostaRepository _cardKanbanSituacaoPrePropostaRepository { get; set; }
        public CardKanbanSituacaoPrePropostaValidator()
        {
            RuleFor(x => x.CardKanbanPreProposta.Id).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Card));
            RuleFor(x => x.StatusPreProposta.Id).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.SituacaoPreProposta));
            RuleFor(x => x.StatusPreProposta.Id).Must(x => RelacaoUnica(x)).WithMessage(x=> MsgRelacaoUnica(x));
        }

        public bool RelacaoUnica(long idStatusPreProposta)
        {
            if (idStatusPreProposta.IsEmpty())
            {
                return true;
            }

            return _cardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => x.StatusPreProposta.Id == idStatusPreProposta)
                .IsEmpty();
        }

        public string MsgRelacaoUnica(CardKanbanSituacaoPreProposta cardKanbanSituacaoPreProposta)
        {
            var relacao = _cardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => x.StatusPreProposta.Id == cardKanbanSituacaoPreProposta.StatusPreProposta.Id)
                .Where(x=>x.CardKanbanPreProposta.Id==cardKanbanSituacaoPreProposta.CardKanbanPreProposta.Id)
                .SingleOrDefault();

            if (relacao.HasValue())
            {
                return string.Format("Situação já está relacionada");
            }

            relacao = _cardKanbanSituacaoPrePropostaRepository.Queryable()
                .Where(x => x.StatusPreProposta.Id == cardKanbanSituacaoPreProposta.StatusPreProposta.Id)
                .SingleOrDefault();

            if (relacao.HasValue())
            {
                return string.Format("Situação já está relacionada ao card {0}",relacao.CardKanbanPreProposta.Descricao);
            }

            return "";
        }
    }
}
