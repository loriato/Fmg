using Europa.Data;
using Europa.Extensions;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewCoordenadorHouseRepository:NHibernateRepository<ViewCoordenadorHouse>
    {
        public DataSourceResponse<ViewCoordenadorHouse> ListarCoordenadorHouse(DataSourceRequest request,FiltroHierarquiaHouseDto filtro)
        {
            var query = Queryable();

            return query.ToDataRequest(request);
        }
    }
}
