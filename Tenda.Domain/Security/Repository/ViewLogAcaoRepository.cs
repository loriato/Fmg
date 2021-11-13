using Europa.Data;
using NHibernate;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewLogAcaoRepository : NHibernateRepository<ViewLogAcao>
    {
        public ViewLogAcaoRepository(ISession session) : base(session)
        {
        }
    }
}
