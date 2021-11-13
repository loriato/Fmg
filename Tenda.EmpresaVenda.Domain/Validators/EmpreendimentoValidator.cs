using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EmpreendimentoValidator : AbstractValidator<Empreendimento>
    {
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }

        public EmpreendimentoValidator(EmpreendimentoRepository empreendimentoRepository)
        {
            _empreendimentoRepository = empreendimentoRepository;

            RuleFor(empr => empr.Nome).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(empr => empr.Regional).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional));
            RuleFor(empr => empr.CNPJ).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cnpj));
            RuleFor(empr => empr.CodigoEmpresa).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CodigoEmpresa));
        }
    }
}