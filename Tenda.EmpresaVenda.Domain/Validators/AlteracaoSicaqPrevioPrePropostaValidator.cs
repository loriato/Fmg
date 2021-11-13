using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AlteracaoSicaqPrevioPrePropostaValidator: AbstractValidator<PreProposta>
    {
        public AlteracaoSicaqPrevioPrePropostaValidator()
        {
            //RuleFor(prep => prep.StatusSicaq).Must(x=>ValidarStatusSicaq(x)).OverridePropertyName("StatusSicaq")
            //    .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.StatusSicaqPrevio));
            RuleFor(prep => prep.ParcelaAprovadaPrevio).Must(x=>ValidarParcela(x)).OverridePropertyName("ParcelaAprovada")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ParcelaAprovada));
            RuleFor(prep=>prep.DataSicaqPrevio).Must(x=>ValidarDataFutura(x)).OverridePropertyName("DataSicaq")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataSicaq));
            RuleFor(prep => prep.FaixaUmMeioPrevio).Must(x=>ValidarFaixaUmMeio(x)).OverridePropertyName("FaixaUmMeio")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.FaixaUmMeio));
        }

        public bool ValidarDataFutura(DateTime? data)
        {
            if (data.IsEmpty() || data.Value.Date > DateTime.Now.Date)
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

        public bool ValidarParcela(decimal? valorParcela)
        {
            if (valorParcela.IsEmpty() || valorParcela.Value == 0)
            {
                return false;
            }

            return true;
        }

        public bool ValidarStatusSicaq(StatusSicaq? sicaq)
        {
            if (sicaq.IsEmpty())
            {
                return false;
            }

            if (sicaq.Value == 0)
            {
                return false;
            }

            return true;
        }
    }
}
