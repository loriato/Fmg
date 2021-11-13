using Europa.Data;
using NHibernate;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class ParametroSistemaRepository : NHibernateRepository<ParametroSistema>
    {
        public ParametroSistemaRepository(ISession session) : base(session)
        {
        }
    }
}
