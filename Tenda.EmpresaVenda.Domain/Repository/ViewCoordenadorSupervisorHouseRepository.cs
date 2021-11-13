using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewCoordenadorSupervisorHouseRepository:NHibernateRepository<ViewCoordenadorSupervisorHouse>
    {

        public DataSourceResponse<ViewCoordenadorSupervisorHouse> ListarSupervisorHouse(DataSourceRequest request,FiltroHierarquiaHouseDto filtro)
        {
            var query = Queryable();

            if (filtro.IdCoordenadorHouse.HasValue())
            {
                if (filtro.Ativo|| filtro.IsSuperiorHouse)
                {
                    query = query.Where(x => x.IdCoordenadorHouse == filtro.IdCoordenadorHouse || x.IdCoordenadorHouse == null);
                }
                else
                {
                    query = query.Where(x => x.IdCoordenadorHouse == filtro.IdCoordenadorHouse);
                }
            }

            if (filtro.IdSupervisorHouse.HasValue())
            {
                query = query.Where(x => x.IdSupervisorHouse == filtro.IdSupervisorHouse);
            }

            if (filtro.NomeSupervisorHouse.HasValue())
            {
                query = query.Where(x => x.NomeSupervisorHouse.ToLower().Contains(filtro.NomeSupervisorHouse));
            }

            return query.ToDataRequest(request);
        }
    }
}
