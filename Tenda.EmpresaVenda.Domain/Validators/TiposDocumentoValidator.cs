using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class TiposDocumentoValidator : AbstractValidator<TipoDocumento>
    {
        public TiposDocumentoValidator()
        {
            RuleFor(x => x).Must(x => x.Nome.HasValue())
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
        }
    }
}
