using Europa.Commons;
using Europa.Data;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class FaseStatusContratoJunixService : BaseService
    {
        FaseStatusContratoJunixRepository _faseStatusContratoJunixRepository { get; set; } 
        public void Salvar(FaseStatusContratoJunix fase, BusinessRuleException bre)
        {
            var validate = new FaseStatusContratoJunixValidator(_faseStatusContratoJunixRepository).Validate(fase);
            bre.WithFluentValidation(validate);

            if (validate.IsValid)
            {
                _faseStatusContratoJunixRepository.Save(fase);
            }
            bre.ThrowIfHasError();
        }
        public FaseStatusContratoJunix ExcluirPorId(FaseStatusContratoJunix fase)
        {
            var exc = new BusinessRuleException();
            try
            {
                _faseStatusContratoJunixRepository.Delete(fase);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(fase.ChaveCandidata()).Complete();
                }
            }
            exc.ThrowIfHasError();
            return fase;
        }
    }
}
