using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ProponenteValidator : AbstractValidator<Proponente>
    {
        public ProponenteRepository _proponenteRepository { get; set; }

        public ProponenteValidator(ProponenteRepository proponenteRepository)
        {
            _proponenteRepository = proponenteRepository;

            RuleFor(prop => prop.PreProposta).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta));
            RuleFor(prop => prop.Cliente).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cliente));
            RuleFor(prop => prop.Participacao).Must(prop => CheckParticipation(prop))
                .WithName("Participacao").WithMessage(GlobalMessages.ValorCampoParticipacaoEntreUmECem);
            RuleFor(prop => prop).Must(prop => CheckClientAlreadyRegistered(prop))
                .WithMessage(GlobalMessages.ProponenteJaCadastrado);
        }

        public bool CheckParticipation(int participation)
        {
            return participation > 0 && participation <= 100;
        }

        public bool CheckClientAlreadyRegistered(Proponente proponente)
        {
            if (proponente.Cliente.IsEmpty())
            {
                return false;
            }

            return !_proponenteRepository.Queryable().Any(x => x.PreProposta.Id == proponente.PreProposta.Id &&
                                                                   x.Cliente.Id == proponente.Cliente.Id &&
                                                                   x.Id != proponente.Id);
        }
    }
}