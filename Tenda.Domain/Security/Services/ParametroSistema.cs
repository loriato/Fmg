using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Linq;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class ParametroSistemaService : BaseService
    {
        public ParametroSistemaRepository _parametroSistemaRepository { get; set; }

        public ParametroSistema BuscarParametro(string codigoSistema)
        {
            return _parametroSistemaRepository.Queryable()
                .Where(reg => reg.Sistema.Codigo.Equals(codigoSistema))
                .Fetch(reg => reg.Sistema).SingleOrDefault();
        }

        public ParametroSistema Salvar(ParametroSistema model)
        {
            BusinessRuleException exception = new BusinessRuleException();
            if (model.Sistema.IsEmpty())
            {
                exception.AddError(GlobalMessages.CampoObrigatorioVazio).WithParams(GlobalMessages.Sistema).Complete();
            }
            exception.ThrowIfHasError();
            _parametroSistemaRepository.Save(model);
            _parametroSistemaRepository.Flush();
            return model;
        }
    }
}
