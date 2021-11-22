using Europa.Data;
using Europa.Extensions;
using Europa.Fmg.Domain.Dto.Viatura;
using System.Linq;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Repository
{
    public class ViaturaRepository : NHibernateRepository<Viatura>
    {
        public DataSourceResponse<Viatura> Listar(DataSourceRequest request, FiltroViaturaDto filtro)
        {
            var query = Queryable();
            if (filtro.Placa.HasValue())
            {
                query = query.Where(x => x.Placa.ToUpper().Contains(filtro.Placa.ToUpper()));
            }
            if (filtro.Renavam.HasValue())
            {
                query = query.Where(x => x.Renavam.Contains(filtro.Renavam));
            }
            if (filtro.Modelo.HasValue())
            {
                query = query.Where(x => x.Modelo.ToUpper().Contains(filtro.Modelo.ToUpper()));
            }
            return query.ToDataRequest(request);
        }
    }
}
