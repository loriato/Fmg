using Europa.Resources;
using FluentValidation;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class DocumentoFormularioDTOValidator : AbstractValidator<DocumentoFormularioDTO>
    {
        public DocumentoFormularioDTOValidator()
        {
            RuleFor(x => x.IdResponsavel).NotEqual(0).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Responsavel));
            RuleFor(x => x.IdPreProposta).NotEqual(0).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta));
            RuleFor(x => x.Formulario).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Formulario));
        }
    }
}
