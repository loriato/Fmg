using Europa.Data;
using NHibernate;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewPerfilPermissaoRepository : NHibernateRepository<ViewPerfilPermissao>
    {
        public ViewPerfilPermissaoRepository(ISession session) : base(session)
        {
        }
    }
}
