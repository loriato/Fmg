﻿using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
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
    public class FornecedorService : BaseService
    {
        private FornecedorRepository _fornecedorRepository { get; set; }

        public void Salvar(Fornecedor model, BusinessRuleException bre)
        {
            model.CNPJ = model.CNPJ.OnlyNumber();

            var validation = new FornecedorValidator(_fornecedorRepository).Validate(model);
            bre.WithFluentValidation(validation);

            if (validation.IsValid)
            {
                _fornecedorRepository.Save(model);
            }
            bre.ThrowIfHasError();
        }

        public Fornecedor Excluir(Fornecedor model)
        {
            var exc = new BusinessRuleException();
            try
            {
                _fornecedorRepository.Delete(model);
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
