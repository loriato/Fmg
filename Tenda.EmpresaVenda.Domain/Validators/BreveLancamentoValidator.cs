using Europa.Resources;
using FluentValidation;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class BreveLancamentoValidator : AbstractValidator<BreveLancamento>
    {
        public BreveLancamentoRepository _breveLancamentoRepository { get; set; }

        public BreveLancamentoValidator(BreveLancamentoRepository breveLancamentoRepository)
        {
            _breveLancamentoRepository = breveLancamentoRepository;
            
            RuleFor(brla => brla.Nome).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));

            RuleFor(brla => brla.Regional.Id).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional));
        }
    }
}