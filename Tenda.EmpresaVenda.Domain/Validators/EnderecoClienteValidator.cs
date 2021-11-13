using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EnderecoClienteValidator : AbstractValidator<EnderecoCliente>
    {
        public EnderecoClienteRepository _enderecoClienteRepository { get; set; }

        public EnderecoClienteValidator(EnderecoClienteRepository enderecoClienteRepository)
        {
            _enderecoClienteRepository = enderecoClienteRepository;

            // Dados Endereço
            RuleFor(emve => emve.Cep).NotEmpty().OverridePropertyName("EnderecoCliente.Cep").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP));
            RuleFor(emve => emve.Logradouro).NotEmpty().OverridePropertyName("EnderecoCliente.Logradouro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco));
            RuleFor(emve => emve.Numero).NotEmpty().OverridePropertyName("EnderecoCliente.Numero").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero));
            RuleFor(emve => emve.Bairro).NotEmpty().OverridePropertyName("EnderecoCliente.Bairro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro));
            RuleFor(emve => emve.Cidade).NotEmpty().OverridePropertyName("EnderecoCliente.Cidade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade));
            RuleFor(emve => emve.Estado).NotEmpty().OverridePropertyName("EnderecoCliente.Estado").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
        }
    }
}
