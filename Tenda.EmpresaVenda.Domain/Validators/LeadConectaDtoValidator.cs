using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using FluentValidation;
using System;
using System.Text.RegularExpressions;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Integration.Conecta.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class LeadConectaDtoValidator : AbstractValidator<LeadConectaDto>
    {
        
        public LeadConectaDtoValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().OverridePropertyName("Nome")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));

            When(x => x.Telefone.IsEmpty(), () =>
            {
                RuleFor(x => x.Telefone).NotEmpty().OverridePropertyName("Telefone")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Telefone));
            }).Otherwise(() =>
            {
                RuleFor(x => x.Telefone).Must(x => TelefoneValido(x))
                .OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(string.Format(GlobalMessages.CampoInvalido, GlobalMessages.Telefone, GlobalMessages.Numero));
            });

            When(x => x.Email.HasValue(), () =>
            {
                RuleFor(x => x.Email).EmailAddress().OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(GlobalMessages.EmailInvalido);
                RuleFor(x => x.Email).Must(x => ValidarEmail(x)).OverridePropertyName("Email")
                .WithMessage(string.Format(GlobalMessages.EmailInvalido));
            }).Otherwise(()=> {
                RuleFor(x => x.Email).NotEmpty().OverridePropertyName("Email")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Email));
            });
            
            RuleFor(x => x.Cpf).NotEmpty().OverridePropertyName("Cpf")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cpf));
            RuleFor(x => x.Cpf).Must(x => CheckIfIsValidCpfCnpj(x)).OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.CpfCnpj));

            RuleFor(x => x.Uuid).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Uuid));

            When(x => x.TelefoneLead.IsEmpty(), () =>
            {
                RuleFor(x => x.TelefoneLead).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TelefoneLead));
            }).Otherwise(() =>
            {
                RuleFor(x => x.TelefoneLead).Must(x => TelefoneValido(x))
                .OverridePropertyName(RestDefinitions.GlobalMessageErrorKey)
                .WithMessage(string.Format(GlobalMessages.CampoInvalido, GlobalMessages.TelefoneLead, GlobalMessages.Numero));
            });
            
            RuleFor(x => x.NomeLead).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeLead));         
            
        }

        public bool CheckIfIsValidCpfCnpj(string cpf)
        {
            if (cpf.IsEmpty())
            {
                return true;
            }
            return cpf.IsValidCPF() || cpf.IsValidCNPJ();
        }

        public bool TelefoneValido(string telefone)
        {
            if (telefone.IsEmpty())
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
