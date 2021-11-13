using Europa.Data.Model;
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
    public class SinteseStatusContratoJunixValidator : AbstractValidator<SinteseStatusContratoJunix>
    {
        public SinteseStatusContratoJunixRepository _sinteseStatusContratoJunixRepository { get; set; }

        public SinteseStatusContratoJunixValidator(SinteseStatusContratoJunixRepository sinteseStatusContratoJunixRepository)
        {
            _sinteseStatusContratoJunixRepository = sinteseStatusContratoJunixRepository;

            RuleFor(x => x.Sintese).NotEmpty().OverridePropertyName("Sintese").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Sintese));
            RuleFor(x => x.Sintese).MaximumLength(DatabaseStandardDefinitions.FiftyLength)
                .OverridePropertyName("Sintese")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.Sintese, DatabaseStandardDefinitions.FiftyLength));

            RuleFor(x => x.StatusContrato).NotEmpty().OverridePropertyName("StatusContrato").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.StatusContrato));
            RuleFor(x => x.StatusContrato).MaximumLength(DatabaseStandardDefinitions.FiftyLength)
                .OverridePropertyName("StatusContrato")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.StatusContrato, DatabaseStandardDefinitions.FiftyLength));

            RuleFor(x => x.FaseJunix.Id).NotEmpty().OverridePropertyName("FaseJunix.Id").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.FaseJunix));
                       
            RuleFor(x => x).Must(x => !CheckIfExistsSinteseStatusContratoJunix(x)).OverridePropertyName("Sintese").WithMessage(string.Format(GlobalMessages.MsgErroRegistroInformada, GlobalMessages.SinteseStatusContratoJunix, GlobalMessages.Sintese));
        }
        public bool CheckIfExistsSinteseStatusContratoJunix(SinteseStatusContratoJunix sintese)
        {
            if (sintese.Sintese.IsEmpty())
            {
                return false;
            }

            if (sintese.FaseJunix.Id.IsEmpty())
            {
                return false;
            }

            return _sinteseStatusContratoJunixRepository.CheckIfExistsSinteseStatusContratoJunix(sintese);
        }
    }
}
