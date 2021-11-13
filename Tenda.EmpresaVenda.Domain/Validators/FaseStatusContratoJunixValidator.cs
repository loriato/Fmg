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
    public class FaseStatusContratoJunixValidator : AbstractValidator<FaseStatusContratoJunix>
    {
        public FaseStatusContratoJunixRepository _faseStatusContratoJunixRepository { get; set; }

        public FaseStatusContratoJunixValidator(FaseStatusContratoJunixRepository faseStatusContratoJunixRepository)
        {
            _faseStatusContratoJunixRepository = faseStatusContratoJunixRepository;

            RuleFor(x => x.Fase).NotEmpty().OverridePropertyName("Fase").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.FaseJunix));
            RuleFor(x => x.Fase).MaximumLength(DatabaseStandardDefinitions.FiftyLength)
                .OverridePropertyName("Fase")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.FaseJunix, DatabaseStandardDefinitions.FiftyLength));
            RuleFor(x => x).Must(x => !CheckIfExistsFaseStatusContratoJunix(x)).OverridePropertyName("Fase").WithMessage(string.Format(GlobalMessages.MsgErroRegistroInformada, GlobalMessages.FaseStatusContratoJunix, GlobalMessages.FaseJunix));
        }
        public bool CheckIfExistsFaseStatusContratoJunix(FaseStatusContratoJunix model)
        {
            if (model.Fase.IsEmpty())
            {
                return false;
            }
            return _faseStatusContratoJunixRepository.CheckIfExistsFaseStatusContratoJunix(model);
        }
    }
}
