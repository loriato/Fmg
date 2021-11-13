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
    public class ClassificacaoService : BaseService
    {
        private ClassificacaoRepository _classificacaoRepository { get; set; }

        public void Salvar(Classificacao model, BusinessRuleException bre)
        {
            var validation = new ClassificacaoValidator(_classificacaoRepository).Validate(model);
            bre.WithFluentValidation(validation);

            if (validation.IsValid)
            {
                _classificacaoRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }

        public Classificacao Excluir(Classificacao model)
        {
            var exc = new BusinessRuleException();
            try
            {
                _classificacaoRepository.Delete(model);
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
