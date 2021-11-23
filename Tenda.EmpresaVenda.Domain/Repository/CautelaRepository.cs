using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Repository
{
    public class CautelaRepository : NHibernateRepository<Cautela>
    {
        public DataSourceResponse<Cautela> Listar(DataSourceRequest request, string nome)
        {
            var query = Queryable();
            if (nome.HasValue())
            {
                query = query.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper())); ;
            }
            return query.ToDataRequest(request);
        }
    }
}
