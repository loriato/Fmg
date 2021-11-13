using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewProponenteRepository : NHibernateRepository<ViewProponente>
    {
        public List<ViewProponente> ListarDaPreProposta(long idPreProposta, long? idCliente)
        {
            var query = Queryable()
                .Where(reg => reg.IdPreProposta == idPreProposta);

            if (idCliente.HasValue())
            {
                query = query.Where(reg => reg.IdCliente == idCliente);
            }

            return query.ToList();
        }

        public DataSourceResponse<ViewProponente> ProponentesDaProposta(DataSourceRequest request, long idPreProposta)
        {
            return Queryable()
                .Where(reg => reg.IdPreProposta == idPreProposta)
                .ToDataRequest(request);
        }
    }
}
