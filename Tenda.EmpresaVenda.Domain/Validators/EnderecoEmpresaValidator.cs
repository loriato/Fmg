using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EnderecoEmpresaValidator : AbstractValidator<EnderecoEmpresa>
    {
        public EnderecoEmpresaRepository _enderecoEmpresaRepository { get; set; }

        public EnderecoEmpresaValidator(EnderecoEmpresaRepository enderecoEmpresaRepository)
        {
            _enderecoEmpresaRepository = enderecoEmpresaRepository;

            // Dados Endereço
            RuleFor(emve => emve.Cep).NotEmpty().OverridePropertyName("EnderecoEmpresa.Cep").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP));
            RuleFor(emve => emve.Logradouro).NotEmpty().OverridePropertyName("EnderecoEmpresa.Logradouro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco));
            RuleFor(emve => emve.Numero).NotEmpty().OverridePropertyName("EnderecoEmpresa.Numero").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero));
            RuleFor(emve => emve.Bairro).NotEmpty().OverridePropertyName("EnderecoEmpresa.Bairro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro));
            RuleFor(emve => emve.Cidade).NotEmpty().OverridePropertyName("EnderecoEmpresa.Cidade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade));
            RuleFor(emve => emve.Estado).NotEmpty().OverridePropertyName("EnderecoEmpresa.Estado").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
        }
    }
}
