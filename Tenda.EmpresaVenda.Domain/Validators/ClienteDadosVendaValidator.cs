using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ClienteDadosVendaValidator : AbstractValidator<Cliente>
    {
        public ClienteRepository _clienteRepository { get; set; }

        public ClienteDadosVendaValidator(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;

            RuleFor(clie => clie).Must(clie => CheckContacts(clie)).OverridePropertyName("Cliente.TelefoneResidencial").WithMessage(GlobalMessages.MsgErroContato);
            RuleFor(clie => clie.Email).EmailAddress().OverridePropertyName("Cliente.Email").WithMessage(GlobalMessages.EmailInvalido);
            RuleFor(clie => clie.NomeCompleto).NotEmpty().OverridePropertyName("Cliente.NomeCompleto").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(clie => clie.TipoSexo).NotEmpty().OverridePropertyName("Cliente.TipoSexo").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Sexo));
            RuleFor(clie => clie.Profissao).NotEmpty().OverridePropertyName("Cliente.Profissao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Profissao));
            RuleFor(clie => clie.Cargo).NotEmpty().OverridePropertyName("Cliente.Cargo").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cargo));
            RuleFor(clie => clie.Escolaridade).NotEmpty().OverridePropertyName("Cliente.Escolaridade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.GrauInstrucao));
            RuleFor(clie => clie.TipoDocumento).NotEmpty().OverridePropertyName("Cliente.TipoDocumento").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoIdentificacao));
            RuleFor(clie => clie.NumeroDocumento).NotEmpty().OverridePropertyName("Cliente.NumeroDocumento").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NumeroDocumento));
            RuleFor(clie => clie.OrgaoEmissor).NotEmpty().OverridePropertyName("Cliente.OrgaoEmissor").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Emissor));
            RuleFor(clie => clie.EstadoEmissor).NotEmpty().OverridePropertyName("Cliente.EstadoEmissor").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EstadoEmissor));
            RuleFor(clie => clie.DataEmissao).NotEmpty().OverridePropertyName("Cliente.DataEmissao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataEmissao));
            RuleFor(clie => clie.Naturalidade).NotEmpty().OverridePropertyName("Cliente.Naturalidade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Naturalidade));
            RuleFor(clie => clie.Filiacao).NotEmpty().OverridePropertyName("Cliente.Filiacao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Filiacao));
            RuleFor(clie => clie.TipoResidencia).NotEmpty().OverridePropertyName("Cliente.TipoResidencia").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoResidencia));
            RuleFor(clie => clie.TipoRenda).NotEmpty().OverridePropertyName("Cliente.TipoRenda").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoRenda));
            RuleFor(clie => clie.FGTS).NotNull().OverridePropertyName("Cliente.FGTS").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorFgts));
            RuleFor(clie => clie.MesesFGTS).NotNull().OverridePropertyName("Cliente.MesesFGTS").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.MesesFgts));
            RuleFor(clie => clie.PrimeiraReferencia).NotEmpty().OverridePropertyName("Cliente.PrimeiraReferencia").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PrimeiraReferencia));
            RuleFor(clie => clie.TelefonePrimeiraReferencia).NotEmpty().OverridePropertyName("Cliente.TelefonePrimeiraReferencia").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PrimeiroTelefone));
            RuleFor(clie => clie.SegundaReferencia).NotEmpty().OverridePropertyName("Cliente.SegundaReferencia").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.SegundaReferencia));
            RuleFor(clie => clie.TelefoneSegundaReferencia).NotEmpty().OverridePropertyName("Cliente.TelefoneSegundaReferencia").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.SegundoTelefone));
        }

        public bool CheckContacts(Cliente cliente)
        {
            if (cliente.TelefoneResidencial.IsEmpty() && cliente.TelefoneComercial.IsEmpty() && cliente.Email.IsEmpty())
            {
                return false;
            }
            return true;
        }

    }
}
