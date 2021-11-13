using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using FluentValidation;
using System;
using System.Text.RegularExpressions;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ClienteLeadConectaValidator:AbstractValidator<Cliente>
    {
        private ClienteRepository _clienteRepository { get; set; }

        public ClienteLeadConectaValidator()
        {
            RuleFor(x => x).Must(x => !CheckIfExistsCpfCnpj(x)).OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(string.Format("Já existe um registro de {0} com o {1} informado no Portal Loja", GlobalMessages.Cliente, GlobalMessages.CpfCnpj));
            RuleFor(x => x).Must(x => ValidarDataFutura(x.DataNascimento))
                .When(x => !x.DataNascimento.IsEmpty()).OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataNascimento));
            RuleFor(x=>x.TelefoneComercial)
                .Must(x => ValidarTelefone(x)).OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(string.Format(GlobalMessages.CampoInvalido, GlobalMessages.TelefonePrincipal, GlobalMessages.Numero));
            RuleFor(x => x.Email).Must(x => ValidarEmail(x)).OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(string.Format(GlobalMessages.EmailInvalido));

        }

        public bool CheckIfExistsCpfCnpj(Cliente cliente)
        {
            if (cliente.CpfCnpj.IsEmpty())
            {
                return true;
            }
            return _clienteRepository.CheckIfExistsCpfCnpj(cliente);
        }

        public bool ValidarDataFutura(DateTime? data)
        {
            if (data.IsEmpty() || data.Value.Date > DateTime.Today)
            {
                return false;
            }
            return true;
        }

        public bool ValidarTelefone(string telefone)
        {
            if (telefone.OnlyNumber().IsEmpty())
            {
                return true;
            }

            return telefone.OnlyNumber().HasValue();
        }


        public bool ValidarEmail(string email)
        {
            if (email.IsEmpty())
            {
                return true;
            }

            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(email);

            return match.Success;

        }
    }
}
