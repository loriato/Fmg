using System;
using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PlanoPagamentoValidator : AbstractValidator<PlanoPagamento>
    {
        public PlanoPagamentoValidator()
        {
            RuleFor(plan => plan.PreProposta).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PreProposta));
            RuleFor(plan => plan.TipoParcela).NotEmpty()
                .WithName("TipoParcela").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TipoParcela));
            RuleFor(plan => plan.NumeroParcelas).NotEmpty()
                .WithName("NumeroParcelas").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NumeroParcelas));
            RuleFor(plan => plan.ValorParcela).NotEmpty()
                .WithName("ValorParcela").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorParcela));
            RuleFor(plan => plan.Total).NotEmpty()
                .WithName("Total").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Total));
            RuleFor(plan => plan.DataVencimento).NotEmpty()
                .WithName("DataVencimento").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataVencimento));
            RuleFor(plan => plan.DataVencimento).Must(plan => CheckDataVencimento(plan))
                .WithName("DataVencimento").WithMessage(x=>string.Format("A data de vencimento da {0} não pode está no passado",x.TipoParcela.AsString()));
                //.WithName("DataVencimento").WithMessage(GlobalMessages.ErroDataVencimentoPassado);
        }

        public bool CheckDataVencimento(DateTime date)
        {
            return date.HasValue() && date.Date >= DateTime.Now.Date;
        }
    }
}