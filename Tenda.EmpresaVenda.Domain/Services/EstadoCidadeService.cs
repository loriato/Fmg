using Europa.Commons;
using Europa.Data;
using Europa.Resources;
using NHibernate.Exceptions;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EstadoCidadeService:BaseService
    {
        private EstadoCidadeRepository _estadoCidadeRepository { get; set; }
        private EstadoCidadeValidator _estadoCidadeValidator { get; set; }
        public EstadoCidade Salvar(EstadoCidade estadoCidade)
        {
            var bre = new BusinessRuleException();

            var validate = _estadoCidadeValidator.Validate(estadoCidade);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            _estadoCidadeRepository.Save(estadoCidade);

            return estadoCidade;
        }

        public EstadoCidade ExcluirCidade(EstadoCidade estadoCidade)
        {
            var bre = new BusinessRuleException();
            try
            {
                _estadoCidadeRepository.Delete(estadoCidade);
                _session.Transaction.Commit();
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(estadoCidade.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();

            return estadoCidade;
        }

    }
}
