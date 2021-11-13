using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AlteracaoSicaqPrePropostaValidator : AbstractValidator<PreProposta>
    {
        public AlteracaoSicaqPrePropostaValidator()
        {
            RuleFor(prep => prep.StatusSicaq).NotEmpty().OverridePropertyName("StatusSicaq")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.StatusSicaq));
            RuleFor(prep => prep.DataSicaq).NotEmpty().OverridePropertyName("DataSicaq")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataSicaq));
            RuleFor(prep => prep).Must(prep => ValidarDataFutura(prep.DataSicaq)).When(prep => !prep.DataSicaq.IsEmpty()).OverridePropertyName("DataSicaq")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataSicaq));
            RuleFor(prep => prep).Must(prep => ValidarStatusCondicional(prep.StatusSicaq, prep.ParcelaAprovada)).OverridePropertyName("ParcelaAprovada")
                 .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ParcelaAprovada));
            RuleFor(prep => prep.FaixaUmMeio).Must(x => ValidarFaixaUmMeio(x)).OverridePropertyName("FaixaUmMeio")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.FaixaUmMeio));
        }

        public bool ValidarDataFutura(DateTime? data)
        {
            if (data.IsEmpty() || data.Value.Date > DateTime.Today)
            {
                return false;
            }
            return true;
        }
        public bool ValidarStatusCondicional(StatusSicaq sicaq, decimal parcela)
        {
            if(parcela.IsEmpty() && sicaq == StatusSicaq.Condicionado)
            {
                return false;
            }
            return true;
        }

        public bool ValidarFaixaUmMeio(bool? faixa)
        {
            if (faixa.IsEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
