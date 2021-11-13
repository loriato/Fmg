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
    public class StatusConformidadeValidator : AbstractValidator<StatusConformidade>
    {
        public StatusConformidadeRepository _statusConformidadeRepository { get; set; }

        public StatusConformidadeValidator(StatusConformidadeRepository statusConformidadeRepository)
        {
            _statusConformidadeRepository = statusConformidadeRepository;

            RuleFor(pntv => pntv.DescricaoJunix).NotEmpty().OverridePropertyName("DescricaoJunix").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DescricaoJunix));
            RuleFor(pntv => pntv.DescricaoJunix).MaximumLength(DatabaseStandardDefinitions.OneHundredTwentyEigthLength)
                .OverridePropertyName("DescricaoJunix")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.DescricaoJunix, DatabaseStandardDefinitions.OneHundredTwentyEigthLength));
            RuleFor(pntv => pntv).Must(pntv => !CheckIfExistsStatusConformidade(pntv)).OverridePropertyName("DescricaoJunix").WithMessage(string.Format(GlobalMessages.MsgErroRegistroInformada, GlobalMessages.StatusConformidadeStatusEvs, GlobalMessages.DescricaoJunix));
           
            RuleFor(pntv => pntv.DescricaoEvs).NotEmpty().OverridePropertyName("DescricaoEvs").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DescricaoEvs));
            RuleFor(pntv => pntv.DescricaoEvs).MaximumLength(DatabaseStandardDefinitions.OneHundredTwentyEigthLength)
                .OverridePropertyName("DescricaoEvs")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.DescricaoEvs, DatabaseStandardDefinitions.OneHundredTwentyEigthLength));     

        }
        public bool CheckIfExistsStatusConformidade(StatusConformidade model)
        {
            if (model.DescricaoJunix.IsEmpty())
            {
                return false;
            }
            return _statusConformidadeRepository.CheckIfExistsStatusConformidade(model);
        }
    }
}
