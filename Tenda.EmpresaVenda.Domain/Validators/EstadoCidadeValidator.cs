using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EstadoCidadeValidator:AbstractValidator<EstadoCidade>
    {
        private EstadoCidadeRepository _estadoCidadeRepository { get; set; }
        public EstadoCidadeValidator()
        {
            RuleFor(x => x.Estado).NotEmpty().OverridePropertyName(GlobalMessages.Estado).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
            RuleFor(x => x.Cidade).NotEmpty().OverridePropertyName(GlobalMessages.Cidade).WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade));
            RuleFor(x => x).Must(x => CidadeUnica(x)).WithMessage(string.Format("{0} já cadastrada", GlobalMessages.Cidade));
        }

        public bool CidadeUnica(EstadoCidade estadoCidade)
        {
            if (estadoCidade.Cidade.IsEmpty())
            {
                return true;
            }

            if (estadoCidade.Estado.IsEmpty())
            {
                return true;
            }

            return !_estadoCidadeRepository.Queryable()
                .Where(x => x.Id != estadoCidade.Id)
                .Where(x => x.Estado.Equals(estadoCidade.Estado))
                .Where(x => x.Cidade.Equals(estadoCidade.Cidade))
                .Any();

        }
    }
}
