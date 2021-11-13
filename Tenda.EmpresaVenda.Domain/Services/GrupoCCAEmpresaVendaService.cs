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
    public class GrupoCCAEmpresaVendaService:BaseService
    {
        private GrupoCCAEmpresaVendaRepository _grupoCCAEmpresaVendaRepository { get; set; }
        public void Save(GrupoCCAEmpresaVenda grupoCCAEmpresaVenda)
        {
            var bre = new BusinessRuleException();

            var result = new GrupoCCAEmpresaVendaValidator(_grupoCCAEmpresaVendaRepository).Validate(grupoCCAEmpresaVenda);

            bre.WithFluentValidation(result);

            bre.ThrowIfHasError();

            _grupoCCAEmpresaVendaRepository.Save(grupoCCAEmpresaVenda);
        }

        public void Delete(GrupoCCAEmpresaVenda grupoCCAEmpresaVenda)
        {
            var bre = new BusinessRuleException();

            try
            {
                _grupoCCAEmpresaVendaRepository.Delete(grupoCCAEmpresaVenda);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(grupoCCAEmpresaVenda.ChaveCandidata()).Complete();
                }
            }

            bre.ThrowIfHasError();
        }
    }
}
