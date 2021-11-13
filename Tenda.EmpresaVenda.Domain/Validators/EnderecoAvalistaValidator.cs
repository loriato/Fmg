using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EnderecoAvalistaValidator : AbstractValidator<EnderecoAvalista>
    {
        public EnderecoAvalistaRepository _enderecoAvalistaRepository { get; set; }

        public EnderecoAvalistaValidator(EnderecoAvalistaRepository enderecoAvalistaRepository)
        {
            _enderecoAvalistaRepository = enderecoAvalistaRepository;

            // Dados Endereço
            RuleFor(emve => emve.Cep).NotEmpty().OverridePropertyName("EnderecoAvalista.Cep").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP));
            RuleFor(emve => emve.Logradouro).NotEmpty().OverridePropertyName("EnderecoAvalista.Logradouro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco));
            RuleFor(emve => emve.Numero).NotEmpty().OverridePropertyName("EnderecoAvalista.Numero").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero));
            RuleFor(emve => emve.Bairro).NotEmpty().OverridePropertyName("EnderecoAvalista.Bairro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro));
            RuleFor(emve => emve.Cidade).NotEmpty().OverridePropertyName("EnderecoAvalista.Cidade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade));
            RuleFor(emve => emve.Estado).NotEmpty().OverridePropertyName("EnderecoAvalista.Estado").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
        }
    }
}
