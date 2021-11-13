using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class CardKanbanPrePropostaValidator : AbstractValidator<CardKanbanPreProposta>
    {
        public CardKanbanPrePropostaValidator()
        {
            RuleFor(x => x.Descricao).NotEmpty().OverridePropertyName("Descricao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(x => x.Cor).NotEmpty().OverridePropertyName("Cor").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cor));
            
            RuleFor(x => x.AreaKanbanPreProposta.Id).NotEmpty().OverridePropertyName("Situacao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Area));
        }
    }
}
