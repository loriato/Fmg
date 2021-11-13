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
    public class TipoCustoService : BaseService
    {
        private TipoCustoRepository _tipoCustoRepository { get; set; }

        public void Salvar(TipoCusto model, BusinessRuleException bre)
        {
            var validation = new TipoCustoValidator(_tipoCustoRepository).Validate(model);
            bre.WithFluentValidation(validation);

            if (validation.IsValid)
            {
                _tipoCustoRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }

        public TipoCusto Excluir(TipoCusto model)
        {
            var exc = new BusinessRuleException();
            try
            {
                _tipoCustoRepository.Delete(model);
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
