using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class UsuarioPortalValidator : AbstractValidator<UsuarioPortal>
    {
        public UsuarioPortalRepository _usuarioPortalRepository { get; set; }

        public UsuarioPortalValidator(UsuarioPortalRepository usuarioPortalRepository)
        {
            _usuarioPortalRepository = usuarioPortalRepository;

            RuleFor(corr => corr).Must(corr => !CheckIfExistsLogin(corr)).WithName("Login").WithMessage(string.Format(GlobalMessages.MsgErroRegistroExistente, GlobalMessages.Usuario, GlobalMessages.Login + "/" + GlobalMessages.Email));
            RuleFor(corr => corr.Nome).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeCompleto));
            RuleFor(corr => corr.Email).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Email));
            RuleFor(corr => corr.Email).Must(email => CheckIfEmailIsValid(email)).WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Email));
        }

        public bool CheckIfExistsLogin(UsuarioPortal usuarioPortal)
        {
            if (usuarioPortal.Login.IsEmpty())
            {
                return false;
            }
            return _usuarioPortalRepository.VerificarLoginDuplicado(usuarioPortal);
        }

        public bool CheckIfEmailIsValid(string email)
        {
            if (email.IsEmpty())
            {
                return true;
            }
            return email.IsValidEmail();
        }

    }
}
