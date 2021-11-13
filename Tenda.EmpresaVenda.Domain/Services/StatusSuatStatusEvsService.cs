using Europa.Commons;
using Europa.Data;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class StatusSuatStatusEvsService : BaseService
    {
        private StatusSuatStatusEvsRepository _statusSuatStatusEvsRepository { get; set; }

        public void Salvar(StatusSuatStatusEvs model, BusinessRuleException bre)
        {
            var statusSuatEvsResult = new StatusSuatStatusEvsValidator(_statusSuatStatusEvsRepository).Validate(model);
            bre.WithFluentValidation(statusSuatEvsResult);

            if (statusSuatEvsResult.IsValid)
            {
                _statusSuatStatusEvsRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }

        public StatusSuatStatusEvs ExcluirPorId(StatusSuatStatusEvs status)
        {
            var exc = new BusinessRuleException();
            try
            {
                _statusSuatStatusEvsRepository.Delete(status);
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

