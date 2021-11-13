using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PrePropostaValidator : AbstractValidator<PreProposta>
    {
        public PrePropostaValidator()
        {
            RuleFor(prpr => prpr.Cliente).NotEmpty().OverridePropertyName("PreProposta.Cliente.NomeCompleto").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Cliente));
            RuleFor(prpr => prpr.PontoVenda).NotEmpty().OverridePropertyName("PreProposta.PontoVenda").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.PontoVenda));
            RuleFor(prpr => prpr.BreveLancamento).NotEmpty().OverridePropertyName("BreveLancamento").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.EmpresasVendas_Produto));
            RuleFor(prpr => prpr.IdTorre).NotEmpty().OverridePropertyName("PreProposta.IdTorre").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Torre));
            RuleFor(prpr => prpr.ParcelaSolicitada).Must(x => x > 0).OverridePropertyName("PreProposta.ParcelaSolicitada").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.ParcelaSolicitada));

            RuleFor(prpr => prpr.OrigemCliente).NotEmpty().OverridePropertyName("PreProposta.OrigemCliente").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.Origem));
            When(prpr => prpr.OrigemCliente == TipoOrigemCliente.Indicacao && prpr.Id.IsEmpty(), () =>
            {
                When(prpr => ProjectProperties.ValidarCamposOrigemIndicacao, () =>
                {
                    RuleFor(prpr => prpr.NomeIndicador).NotEmpty().OverridePropertyName("PreProposta.NomeIndicador").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.NomeIndicador));
                    RuleFor(prpr => prpr.CpfIndicador).NotEmpty().OverridePropertyName("PreProposta.CpfIndicador").WithMessage(string.Format(GlobalMessages.CampoObrigatorioVazio, GlobalMessages.CpfIndicador));
                });
                When(prpr => !prpr.CpfIndicador.IsEmpty(), () =>
               {
                   RuleFor(prpr => prpr.CpfIndicador)
                       .Must(prpr => prpr.IsValidCPF())
                       .OverridePropertyName("PreProposta.CpfIndicador")
                       .WithMessage(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.CpfIndicador));
               });
            });

        }

        public bool IsItBreveLancamento(PreProposta preProposta)
        {
            if (preProposta.EmpresaVenda.Loja.HasValue())
            {
                return true;
            }

            if (preProposta.SituacaoProposta == SituacaoProposta.AguardandoFluxo)
            {
                return true;
            }

            //if (!preProposta.IsBreveLancamento)
            //{
            //    return true;
            //}

            if (preProposta.BreveLancamento.HasValue())
            {
                return true;
            }

            return false;
        }
    }
}
