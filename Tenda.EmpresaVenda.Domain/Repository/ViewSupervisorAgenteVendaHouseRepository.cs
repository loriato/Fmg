using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewSupervisorAgenteVendaHouseRepository:NHibernateRepository<ViewSupervisorAgenteVendaHouse>
    {
        public DataSourceResponse<ViewSupervisorAgenteVendaHouse> ListarAgenteVendaHouse(DataSourceRequest request, FiltroHierarquiaHouseDto filtro)
        {
            var query = Queryable();

            if (filtro.IdSupervisorHouse.HasValue())
            {
                if (filtro.Ativo || filtro.IsSuperiorHouse)
                {
                    query = query.Where(x => x.IdSupervisorHouse == filtro.IdSupervisorHouse || x.IdSupervisorHouse == null);
                }
                else
                {
                    query = Queryable().Where(x => x.IdSupervisorHouse == filtro.IdSupervisorHouse);
                }
            }
            
            if (filtro.NomeAgenteVenda.HasValue())
            {
                query = query.Where(x => x.NomeUsuarioAgenteVenda.ToLower().Contains(filtro.NomeAgenteVenda.ToLower()));
            }

            if (filtro.NomeHouse.HasValue())
            {
                query = query.Where(x =>x.NomeHouse!=null && x.NomeHouse.ToLower().Contains(filtro.NomeHouse));
            }

            return query.ToDataRequest(request);
        }
    }
}
