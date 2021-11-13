using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EnderecoEmpreendimentoValidator : AbstractValidator<EnderecoEmpreendimento>
    {
        public EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }

        public EnderecoEmpreendimentoValidator(EnderecoEmpreendimentoRepository enderecoEmpreendimentoRepository)
        {
            _enderecoEmpreendimentoRepository = enderecoEmpreendimentoRepository;

            // Dados Endereço
            RuleFor(emve => emve.Cep).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP));
            RuleFor(emve => emve.Logradouro).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco));
            RuleFor(emve => emve.Bairro).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro));
            RuleFor(emve => emve.Cidade).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade));
            RuleFor(emve => emve.Estado).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
        }
    }
}
