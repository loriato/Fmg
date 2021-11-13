using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.EnvioPreProposta
{
    public class ProponenteValidator : AbstractValidator<Proponente>
    {
        public ProponenteValidator()
        {
            string mensagemCampoObrigatorio = "O campo {0} é obrigatório e não está preenchido para o cliente {1}";

            RuleFor(prop => prop.Cliente.NomeCompleto).NotEmpty().OverridePropertyName(prop => prop.Cliente.NomeCompleto).WithMessage(reg => string.Format(mensagemCampoObrigatorio, GlobalMessages.NomeCompleto, reg.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.TipoRenda).NotEmpty().OverridePropertyName("propnte.TipoRenda").WithMessage(reg => string.Format(mensagemCampoObrigatorio, GlobalMessages.TipoRenda, reg.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.CpfCnpj).NotEmpty().OverridePropertyName("propnte.CpfCnpj").WithMessage(reg => string.Format(mensagemCampoObrigatorio, GlobalMessages.CpfCnpj, reg.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.DataNascimento).NotEmpty().OverridePropertyName("propnte.CpfCnpj").WithMessage(reg => string.Format(mensagemCampoObrigatorio, GlobalMessages.DataNascimento, reg.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.RendaMensal).NotEmpty().OverridePropertyName("propnte.CpfCnpj").WithMessage(reg => string.Format(mensagemCampoObrigatorio, GlobalMessages.RendaMensal, reg.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.MesesFGTS).NotNull().OverridePropertyName("propnte.MesesFGTS").WithMessage(reg => string.Format(mensagemCampoObrigatorio, GlobalMessages.MesesFgts, reg.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.FGTS).NotEmpty().OverridePropertyName("propnte.FGTS").WithMessage(reg => string.Format(mensagemCampoObrigatorio, GlobalMessages.FGTS, reg.Cliente.NomeCompleto));
        }
    }
}