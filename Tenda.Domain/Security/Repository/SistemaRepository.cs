using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Linq;
using Tenda.Domain.Security.Models;


namespace Tenda.Domain.Security.Repository
{
    public class SistemaRepository : NHibernateRepository<Sistema>
    {
        public SistemaRepository(ISession session) : base(session)
        {
        }

        public IQueryable<Sistema> Listar(DataSourceRequest request)
        {
            var query = Queryable();
            if (request.filter.FirstOrDefault().HasValue())
            {
                var filtroNome = request.filter.FirstOrDefault(reg => reg.column.ToLower() == "nome");
                query = query.Where(x => x.Nome.ToLower().Contains(filtroNome.value.ToString().ToLower()));
            }
            return query;
        }

        public Sistema FindByCodigo(string codigoSistema)
        {
            return Queryable()
                .Where(reg => reg.Codigo.ToUpper() == codigoSistema.ToUpper())
                .SingleOrDefault();
        }
    }
}
