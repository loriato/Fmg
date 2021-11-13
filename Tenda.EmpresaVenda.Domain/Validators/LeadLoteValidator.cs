using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class LeadLoteValidator : AbstractValidator<LeadDTO>
    {
        public LeadLoteValidator()
        {
            RuleFor(x => x.IdsLeadsEmpresasVendas).Must(x => ValidarLead(x)).WithMessage("Selecione um lead");
            RuleFor(x => x.IdCorretor).NotEmpty().WithMessage("É obrigatório a escolha de um corretor");            
        }

        public bool ValidarLead(List<long> leads)
        {
            return leads.Count > 0;
        }
    }
}
