using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class GrupoCCAEmpresaVendaValidator:AbstractValidator<GrupoCCAEmpresaVenda>
    {
        private GrupoCCAEmpresaVendaRepository _grupoCCAEmpresaVendaRepository { get; set; }

        public GrupoCCAEmpresaVendaValidator(GrupoCCAEmpresaVendaRepository grupoCCAEmpresaVendaRepository)
        {
            _grupoCCAEmpresaVendaRepository = grupoCCAEmpresaVendaRepository;

            RuleFor(x => x.GrupoCCA).Must(x => ValidarGrupoCCA(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.CCA));
            RuleFor(x => x.EmpresaVenda).Must(x => ValidarEmpresaVenda(x)).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EmpresaVenda));
        }

        public bool ValidarGrupoCCA(GrupoCCA grupo)
        {
            return !grupo.IsEmpty();
        }

        public bool ValidarEmpresaVenda(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            return !empresaVenda.IsEmpty();
        }
    }
}
