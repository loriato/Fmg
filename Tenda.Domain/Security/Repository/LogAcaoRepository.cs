using Europa.Data;
using NHibernate;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class LogAcaoRepository : NHibernateRepository<LogAcao>
    {
        public LogAcaoRepository(ISession session) : base(session)
        {
        }
    }
}
