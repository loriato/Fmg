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
    public class CentroCustoService : BaseService
    {
        private CentroCustoRepository _centroCustoRepository { get; set; }

        public void Salvar(CentroCusto model, BusinessRuleException bre)
        {
            var validation = new CentroCustoValidator(_centroCustoRepository).Validate(model);
            bre.WithFluentValidation(validation);

            if (validation.IsValid)
            {
                _centroCustoRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }

        public CentroCusto Excluir(CentroCusto model)
        {
            var exc = new BusinessRuleException();
            try
            {
                _centroCustoRepository.Delete(model);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(model.ChaveCandidata()).Complete();
                }
            }
            exc.ThrowIfHasError();
            return model;
        }
    }
}
