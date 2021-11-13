using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewCoordenadorHouseRepsository:NHibernateRepository<ViewCoordenadorHouse>
    {
        public DataSourceResponse<ViewCoordenadorHouse> ListarCoordenadorHouse(DataSourceRequest request,FiltroHierarquiaHouseDto filtro)
        {
            var query = Queryable();

            if (filtro.NomeCoordenadorHouse.HasValue())
            {
                query = query.Where(x => x.NomeCoordenadorHouse.ToLower().Contains(filtro.NomeCoordenadorHouse));
            }

            return query.ToDataRequest(request);
        }
    }
}
