using Europa.Data;
using NHibernate;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class QuartzConfigurationRepository : NHibernateRepository<QuartzConfiguration>
    {
        public QuartzConfigurationRepository(ISession session) : base(session)
        {
        }

        public QuartzConfiguration FindByName(string nome)
        {
            var query = Queryable();
            query = query.Where(x=>x.Nome.ToLower().Equals(nome.ToLower()));
            return query.FirstOrDefault();
        }
    }
}
