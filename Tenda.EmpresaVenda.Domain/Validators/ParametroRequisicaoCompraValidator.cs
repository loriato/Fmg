using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ParametroRequisicaoCompraValidator : AbstractValidator<ParametroRequisicaoCompra>
    {
        public ParametroRequisicaoCompraValidator()
        {

            RuleFor(reg => reg.Valor).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Valor));
        }
    }
}
