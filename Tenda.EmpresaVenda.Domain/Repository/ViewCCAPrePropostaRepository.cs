using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    class ViewCCAPrePropostaRepository : NHibernateRepository<ViewCCAPreProposta>
    {
        public DataSourceResponse<ViewCCAPreProposta> BuscarCCAsPreProposta(DataSourceRequest request, long IdPreProposta)
        {
            var query = Queryable()
                        .Where(x => x.IdPreProposta == IdPreProposta);
            return query.ToDataRequest(request);
        }

        public List<ViewCCAPreProposta> BuscarCCAsPreProposta(long IdPreProposta)
        {
            var query = Queryable()
                        .Where(x => x.IdPreProposta == IdPreProposta);
            return query.ToList();
        }
    }
}
