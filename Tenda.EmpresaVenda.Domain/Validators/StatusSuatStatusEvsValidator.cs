using Europa.Data.Model;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class StatusSuatStatusEvsValidator : AbstractValidator<StatusSuatStatusEvs>
    {
        public StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }

        public StatusSuatStatusEvsValidator(StatusSuatStatusEvsRepository statusSuatStatusEvsRepository)
        {
            _statusSuatStatusEvsRepository = statusSuatStatusEvsRepository;

            RuleFor(pntv => pntv.DescricaoSuat).NotEmpty().OverridePropertyName("DescricaoSuat").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DescricaoSuat));
            RuleFor(pntv => pntv.DescricaoSuat).MaximumLength(DatabaseStandardDefinitions.FiftyLength)
                .OverridePropertyName("DescricaoSuat")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.DescricaoSuat, DatabaseStandardDefinitions.FiftyLength));
            RuleFor(pntv => pntv).Must(pntv => !CheckIfExistsStatusSuat(pntv)).OverridePropertyName("DescricaoSuat").WithMessage(string.Format(GlobalMessages.MsgErroRegistroInformada, GlobalMessages.StatusSuatStatusEvs, GlobalMessages.DescricaoSuat));

    
            RuleFor(pntv => pntv.DescricaoEvs).NotEmpty().OverridePropertyName("DescricaoEvs").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DescricaoEvs));
            RuleFor(pntv => pntv.DescricaoEvs).MaximumLength(DatabaseStandardDefinitions.FiftyLength)
                .OverridePropertyName("DescricaoEvs")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.DescricaoEvs, DatabaseStandardDefinitions.FiftyLength));

        }
        public bool CheckIfExistsStatusSuat(StatusSuatStatusEvs model)
        {
            if (model.DescricaoSuat.IsEmpty())
            {
                return false;
            }
            return _statusSuatStatusEvsRepository.CheckIfExistsStatusSuat(model);
        }
    }
}
