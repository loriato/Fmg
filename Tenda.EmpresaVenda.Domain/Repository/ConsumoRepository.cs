using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Repository
{
    public class ConsumoRepository : NHibernateRepository<Consumo>
    {
        public DataSourceResponse<Consumo> Listar(DataSourceRequest request, string nome)
        {
            var query = Queryable();
            if (nome.HasValue())
            {
                query = query.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper())); ;
            }
            return query.ToDataRequest(request);
        }
        public DataSourceResponse<Consumo> ListarConsumoFalta(DataSourceRequest request)
        {
            var query = Queryable().Where(x => x.Total <= 0);

            return query.ToDataRequest(request);
        }
    }
}
