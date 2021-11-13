using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.HierarquiaHouse;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class HierarquiaHouseRepository:NHibernateRepository<HierarquiaHouse>
    {
        public IQueryable<HierarquiaHouse> QueryHierarquiaHouse(FiltroHierarquiaHouseDto filtro)
        {
            var query = Queryable();

            if (filtro.IdCoordenadorHouse.HasValue())
            {
                query = query.Where(x => x.Coordenador.Id == filtro.IdCoordenadorHouse);
            }

            if (filtro.IdSupervisorHouse.HasValue())
            {
                query = query.Where(x => x.Supervisor.Id == filtro.IdSupervisorHouse);
            }

            if (filtro.IdAgenteVenda.HasValue())
            {
                query = query.Where(x => x.AgenteVenda.Id == filtro.IdAgenteVenda);
            }

            if (filtro.IdHouse.HasValue())
            {
                query = query.Where(x => x.House.Id == filtro.IdHouse);
            }

            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => x.Situacao == filtro.Situacao);
            }

            return query;
        }

        public List<HierarquiaHouse> ListarHierarquiaHouse(FiltroHierarquiaHouseDto filtro)
        {
            var lista = QueryHierarquiaHouse(filtro).ToList();
            return lista;
        }

        public HierarquiaHouse BuscarHierarquiaHouse(FiltroHierarquiaHouseDto filtro)
        {
            var hierarquiaHouse = ListarHierarquiaHouse(filtro).SingleOrDefault();
            return hierarquiaHouse;
        }
    }
}
