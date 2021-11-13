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
    public class SinteseStatusContratoJunixService : BaseService
    {
        SinteseStatusContratoJunixRepository _sinteseStatusContratoJunixRepository { get; set; }
        public void Salvar(SinteseStatusContratoJunix sintese, BusinessRuleException bre)
        {
            var validate = new SinteseStatusContratoJunixValidator(_sinteseStatusContratoJunixRepository).Validate(sintese);
            bre.WithFluentValidation(validate);

            if (validate.IsValid)
            {
                _sinteseStatusContratoJunixRepository.Save(sintese);
            }
            bre.ThrowIfHasError();
        }

        public SinteseStatusContratoJunix ExcluirPorId(SinteseStatusContratoJunix sintese)
        {
            var exc = new BusinessRuleException();
            try
            {
                _sinteseStatusContratoJunixRepository.Delete(sintese);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(sintese.ChaveCandidata()).Complete();
                }
            }
            exc.ThrowIfHasError();
            return sintese;
        }
    }
    
}
