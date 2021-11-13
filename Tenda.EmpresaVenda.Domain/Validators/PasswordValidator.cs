using Europa.Resources;
using FluentValidation;
using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PasswordValidator : AbstractValidator<KeyValuePair<string, string>>
    {

        public PasswordValidator()
        {
            RuleFor(corr => corr.Key).NotEmpty().WithName("NovaSenha").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Senha));
            RuleFor(corr => corr.Key).MinimumLength(8).WithName(GlobalMessages.NovaSenha).OverridePropertyName("NovaSenha");
            RuleFor(corr => corr).Must(keyPair => IsEqual(keyPair)).WithName("ConfirmeSenha").WithMessage(GlobalMessages.SenhaConfirmacaoDiferentes);
        }

        public bool IsEqual(KeyValuePair<string, string> keyPair)
        {
            return keyPair.Key == keyPair.Value;
        }
    }
}
