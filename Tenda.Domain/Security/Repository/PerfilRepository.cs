using Europa.Data;
using NHibernate;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class PerfilRepository : NHibernateRepository<Perfil>
    {
        public PerfilRepository(ISession session) : base(session)
        {
        }
    }
}
