using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EstadoAvalistaValidator : AbstractValidator<EstadoAvalista>
    {
        public  EstadoAvalistaRepository _estadoAvalistaRepository { get; set; }
        public EstadoAvalistaValidator(EstadoAvalistaRepository usuarioGrupoCCARepository)
        {
            _estadoAvalistaRepository = usuarioGrupoCCARepository;

            RuleFor(x => x.Avalista).Must(x => ValidarUsuario(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Avalista));
            RuleFor(x => x.NomeEstado).Must(x => ValidarNome(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
        }

        public bool ValidarNome(string nome)
        {
            return !nome.IsEmpty();
        }

        public bool ValidarUsuario(UsuarioPortal usuario)
        {
            return !usuario.IsEmpty();
        }
    }
}
