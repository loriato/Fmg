using Europa.Commons;
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
    public class ImportacaoJunixService : BaseService
    {
        public ImportacaoJunixRepository _importacaoJunixRepository { get; set; }
        public void Salvar(ImportacaoJunix importacaoJunix)
        {
            BusinessRuleException bre = new BusinessRuleException();
            var validate = new ImportacaoJunixValidator(_importacaoJunixRepository).Validate(importacaoJunix);
            bre.WithFluentValidation(validate);

            if (validate.IsValid)
            {                
                _importacaoJunixRepository.Save(importacaoJunix);
            }
            bre.ThrowIfHasError();
        }
    }
}
