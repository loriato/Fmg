using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ImportacaoJunixValidator : AbstractValidator<ImportacaoJunix>
    {
        public ImportacaoJunixRepository _importacaoJunixRepository { get; set; }

        public ImportacaoJunixValidator(ImportacaoJunixRepository importacaoJunixRepository)
        {
            _importacaoJunixRepository = importacaoJunixRepository;

            RuleFor(x => x.Situacao).NotEmpty().OverridePropertyName("Situacao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Situacao));
            RuleFor(x => x.Origem).NotEmpty().OverridePropertyName("Origem").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Origem));
            RuleFor(x => x.Arquivo).NotEmpty().OverridePropertyName("Arquivo").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Arquivo));
        }
    }
}
