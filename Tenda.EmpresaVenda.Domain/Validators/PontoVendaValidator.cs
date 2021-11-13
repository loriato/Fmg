using Europa.Data.Model;
using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PontoVendaValidator : AbstractValidator<PontoVenda>
    {
        public PontoVendaRepository _pontoVendaRepo { get; set; }

        public PontoVendaValidator(PontoVendaRepository pontoVendaRepo)
        {
            _pontoVendaRepo = pontoVendaRepo;

            RuleFor(pntv => pntv.Nome).NotEmpty().OverridePropertyName("Nome").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(pntv => pntv.Nome).MaximumLength(DatabaseStandardDefinitions.OneHundredTwentyEigthLength)
                .OverridePropertyName("Nome")
                .WithMessage(string.Format(GlobalMessages.TamanhoCampoExcedido, GlobalMessages.Nome, DatabaseStandardDefinitions.OneHundredTwentyEigthLength));
            RuleFor(pntv => pntv).Must(pntv => !CheckIfExistsNomeWithEmpresaVenda(pntv)).OverridePropertyName("Nome").WithMessage(string.Format(GlobalMessages.MsgErroRegistroExistenteEmpresaVenda, GlobalMessages.PontoVenda, GlobalMessages.Nome));

            RuleFor(pntv => pntv.EmpresaVenda).NotEmpty().OverridePropertyName("EmpresaVenda").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EmpresaVenda));
            RuleFor(pntv => pntv.IniciativaTenda).NotNull().OverridePropertyName("IniciativaTenda").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.GerenciadoTenda));
            RuleFor(pntv => pntv.Situacao).NotEmpty().OverridePropertyName("Situacao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Situacao));
        }

        public bool CheckIfExistsNomeWithEmpresaVenda(PontoVenda model)
        {
            if (model.Nome.IsEmpty() || model.EmpresaVenda.IsEmpty())
            {
                return false;
            }
            return _pontoVendaRepo.CheckIfExistsNomeWithEmpresaVenda(model);
        }
    }
}
