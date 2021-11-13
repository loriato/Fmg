using Europa.Data;
using NHibernate;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class UsuarioRepository : NHibernateRepository<Usuario>
    {
        public UsuarioRepository(ISession session) : base(session) { }

        public Usuario FindByLogin(string login)
        {
            return Queryable()
                   .Where(reg => reg.Login.ToLower().Equals(login.ToLower()))
                   .SingleOrDefault();
        }
    }
}
