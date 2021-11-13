using Europa.Data;
using NHibernate;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewLogEntidadeRepository : NHibernateRepository<ViewLogEntidade>
    {
        public ViewLogEntidadeRepository(ISession session) : base(session)
        {
        }
    }
}
