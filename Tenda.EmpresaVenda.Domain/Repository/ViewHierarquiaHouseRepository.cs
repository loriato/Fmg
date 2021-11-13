using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewHierarquiaHouseRepository : NHibernateRepository<ViewHierarquiaHouse>
    {
        public DataSourceResponse<ViewHierarquiaHouse> ListarHierarquiaHouse(DataSourceRequest request,FiltroHierarquiaHouseDto filtro)
        {
            var query = Queryable();

            if (filtro.IdCoordenadorHouse.HasValue())
            {
                query = query.Where(x => x.IdCoordenadorHouse == filtro.IdCoordenadorHouse);
            }

            if (filtro.IdSupervisorHouse.HasValue())
            {
                query = query.Where(x => x.IdSupervisorHouse == filtro.IdSupervisorHouse);
            }

            if (filtro.IdAgenteVenda.HasValue())
            {
                query = query.Where(x => x.IdAgenteVendaHouse == filtro.IdAgenteVenda);
            }

            if (filtro.IdHouse.HasValue())
            {
                query = query.Where(x => x.IdHouse == filtro.IdHouse);
            }

            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => x.Situacao == filtro.Situacao);
            }

            if (filtro.PeriodoDe.HasValue())
            {
                query = query.Where(x => x.Inicio.Date >= filtro.PeriodoDe.Value.Date);
            }

            if (filtro.PeriodoAte.HasValue())
            {
                query = query.Where(x => x.Fim.Value.Date <= filtro.PeriodoAte.Value.Date);
            }

            if (filtro.NomeAgenteVenda.HasValue())
            {
                filtro.NomeAgenteVenda = filtro.NomeAgenteVenda.ToLower();
                query = query.Where(x => x.NomeAgenteVendaHouse.ToLower().Contains(filtro.NomeAgenteVenda));
            }

            return query.ToDataRequest(request);
        }
    }
}
