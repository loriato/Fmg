using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewStandVendaRepository : NHibernateRepository<ViewStandVenda>
    {
        public DataSourceResponse<ViewStandVenda> ListarDatatable(DataSourceRequest request, FiltroStandVendaDTO filtro)
        {
            var query = Queryable();

            if (filtro.Nome.HasValue())
            {
                query = query.Where(x => x.Nome.ToUpper().Contains(filtro.Nome.ToUpper()));
            }
            if (filtro.IdRegional.HasValue())
            {
                query = query.Where(x => filtro.IdRegional == x.IdRegional);
            }
            if (filtro.Estado.HasValue())
            {
                query = query.Where(x => filtro.Estado == x.Estado);
            }


            return query.ToDataRequest(request);
        }
    }
}
