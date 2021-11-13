using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewHistoricoPrePropostaRepository : NHibernateRepository<ViewHistoricoPreProposta>
    {
        public DataSourceResponse<ViewHistoricoPreProposta> Listar(DataSourceRequest request, long idPreProposta)
        {
            return Queryable()
                .Where(reg => reg.IdPreProposta == idPreProposta)
                .ToDataRequest(request);
        }
    }
}
