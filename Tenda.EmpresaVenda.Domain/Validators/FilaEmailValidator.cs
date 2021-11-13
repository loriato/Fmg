using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class FilaEmailValidator : AbstractValidator<FilaEmail>
    {
        public FilaEmailValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Titulo));
            RuleFor(x => x.Mensagem).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Mensagem));
            RuleFor(x => x.Destinatario).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Destinatario));
            RuleFor(x => x.SituacaoEnvio).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.SituacaoEnvio));

        }
    }
}
