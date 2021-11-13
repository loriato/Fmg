using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class LeadEmpresaVendaValidator : AbstractValidator<LeadEmpresaVenda>
    {
        public LeadEmpresaVendaValidator()
        {
            RuleFor(x => x).Must(x => ValidarDescricaoDesistencia(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(x => x).Must(x => ValidarDesistencia(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.SituacaoLead_Desistencia));
        }

        public bool ValidarDesistencia(LeadEmpresaVenda leadEmpresaVenda)
        {
            if (leadEmpresaVenda.Situacao != SituacaoLead.Desistencia)
            {
                return true;
            }

            if (leadEmpresaVenda.Desistencia.IsEmpty())
            {
                return false;
            }

            return true;
        }
        public bool ValidarDescricaoDesistencia(LeadEmpresaVenda leadEmpresaVenda)
        {
            if (leadEmpresaVenda.Desistencia != TipoDesistencia.Outros)
            {
                return true;
            }

            if (leadEmpresaVenda.DescricaoDesistencia.IsEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
