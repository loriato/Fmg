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
    public class StatusConformidadeService : BaseService
    {
        private StatusConformidadeRepository _statusConformidadeRepository { get; set; }

        public void Salvar(StatusConformidade model, BusinessRuleException bre)
        {
            var statusConformidadeEvsResult = new StatusConformidadeValidator(_statusConformidadeRepository).Validate(model);
            bre.WithFluentValidation(statusConformidadeEvsResult);

            if (statusConformidadeEvsResult.IsValid)
            {
                _statusConformidadeRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }

        public StatusConformidade ExcluirPorId(StatusConformidade status)
        {
            var exc = new BusinessRuleException();
            try
            {
                _statusConformidadeRepository.Delete(status);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(status.ChaveCandidata()).Complete();
                }
            }
            exc.ThrowIfHasError();
            return status;
        }
    }
}
