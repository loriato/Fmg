using Europa.Data;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class AcessoPerfilRepository : NHibernateRepository<AcessoPerfil>
    {
        public AcessoPerfilRepository(ISession session) : base(session)
        {
        }

        public List<Perfil> BuscarPerfisDoAcesso(long idAcesso)
        {
            return Queryable()
                .Where(reg => reg.Acesso.Id == idAcesso)
                .Select(reg => reg.Perfil)
                .ToList();
        }
    }
}
