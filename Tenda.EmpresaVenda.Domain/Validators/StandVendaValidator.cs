using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class StandVendaValidator : AbstractValidator<StandVenda>
    {
        public StandVendaValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().OverridePropertyName(x=>x.Nome).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(x => x.Estado).NotEmpty().OverridePropertyName(x=>x.Regional).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
            RuleFor(x => x.IdRegional).NotEmpty().OverridePropertyName(x => x.Regional).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional));
        }
    }
}
