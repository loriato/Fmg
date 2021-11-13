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
    public class GrupoCCAService:BaseService
    {
        private GrupoCCARepository _grupoCCARepository { get; set; }
        public GrupoCCA SalvarGrupoCCA(GrupoCCA grupo)
        {
            var bre = new BusinessRuleException();

            var result = new GrupoCCAValidator(_grupoCCARepository).Validate(grupo);

            bre.WithFluentValidation(result);

            bre.ThrowIfHasError();

            _grupoCCARepository.Save(grupo);

            return grupo;
        }

        public void ExluirGrupoCCAPorId(GrupoCCA grupo)
        {
            var exc = new BusinessRuleException();
            try
            {
                _grupoCCARepository.Delete(grupo);
                _session.Transaction.Commit();
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(grupo.ChaveCandidata()).Complete();
                }
            }

            exc.ThrowIfHasError();
        }
    }
}
