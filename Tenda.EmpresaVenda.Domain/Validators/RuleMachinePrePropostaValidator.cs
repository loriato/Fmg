using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class RuleMachinePrePropostaValidator : AbstractValidator<RuleMachinePreProposta>
    {
        private RuleMachinePrePropostaRepository _ruleMachinePrePropostaRepository { get; set; }
        public RuleMachinePrePropostaValidator()
        {
            RuleFor(x => x.Origem).NotEmpty().OverridePropertyName("Origem").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Origem));
            RuleFor(x => x.Destino).NotEmpty().OverridePropertyName("Destino").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Destino));
            RuleFor(x => x).Must(x => TransicaoUnica(x)).WithMessage(x => string.Format("Transição {0} para {1} {2}",x.Origem.AsString(),x.Destino.AsString(),GlobalMessages.Invalida));
            RuleFor(x => x).Must(x => TransicaoValida(x)).WithMessage(x => string.Format("Transição {0} para {1} {2}", x.Origem.AsString(), x.Destino.AsString(), GlobalMessages.Invalida));
        }

        public bool TransicaoUnica(RuleMachinePreProposta rule)
        {
            if (rule.Origem.IsEmpty())
            {
                return true;
            }

            if (rule.Destino.IsEmpty())
            {
                return true;
            }

            return !_ruleMachinePrePropostaRepository.TransicaoExistente(rule);
        }

        public bool TransicaoValida(RuleMachinePreProposta rule)
        {
            if (rule.Origem.IsEmpty())
            {
                return true;
            }

            if (rule.Destino.IsEmpty())
            {
                return true;
            }

            return rule.Origem != rule.Destino;
        }
    }
}
