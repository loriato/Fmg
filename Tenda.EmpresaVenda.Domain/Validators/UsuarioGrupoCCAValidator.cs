using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class UsuarioGrupoCCAValidator:AbstractValidator<UsuarioGrupoCCA>
    {
        public  UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }
        public UsuarioGrupoCCAValidator(UsuarioGrupoCCARepository usuarioGrupoCCARepository)
        {
            _usuarioGrupoCCARepository = usuarioGrupoCCARepository;

            RuleFor(x => x.GrupoCCA).Must(x => ValidarGrupoCCA(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CCA));
            RuleFor(x => x.Usuario).Must(x => ValidarUsuario(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Usuario));
            RuleFor(x => x.Usuario).Must(x => ValidarCruzamento(x)).WithMessage(x=>MsgErroUsuarioGrupoCCA(x.Usuario.Id));
        }

        public bool ValidarGrupoCCA(GrupoCCA grupo)
        {
            return !grupo.IsEmpty();
        }

        public bool ValidarUsuario(UsuarioPortal usuario)
        {
            return !usuario.IsEmpty();
        }

        public bool ValidarCruzamento(UsuarioPortal usuario)
        {
            if (usuario.HasValue())
            {
                return !_usuarioGrupoCCARepository.Queryable()
                    .Where(x => x.Usuario.Id == usuario.Id)
                    .Any();
            }

            return true;

        }

        public string MsgErroUsuarioGrupoCCA(long idUsuario)
        {
            var usuarioGrupoCCA = _usuarioGrupoCCARepository.Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .FirstOrDefault();

            return usuarioGrupoCCA.HasValue() ? string.Format("O usuario já está associado ao grupo {0}", usuarioGrupoCCA.GrupoCCA.Descricao) : "";

        }
    }
}
