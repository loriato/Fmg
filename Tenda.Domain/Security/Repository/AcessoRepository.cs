using Europa.Data;
using NHibernate;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class AcessoRepository : NHibernateRepository<Acesso>
    {
        public AcessoRepository(ISession session) : base(session)
        {
        }

        public Acesso FindLastAccessByUser(long userSku)
        {
            return Queryable().Where(x => x.Usuario.Id == userSku).OrderByDescending(x => x.InicioSessao)
                .FirstOrDefault();
        }

        public Acesso FindByAuthToken(string userToken)
        {
            return Queryable().Where(reg => reg.Autorizacao == userToken)
                .SingleOrDefault();
        }
        //Retorna um acesso em que a sessão não tenha finalizado
        public Acesso FindByAuthTokenApi(string userToken)
        {
            return Queryable().Where(reg => reg.Autorizacao == userToken)
                .Where(x => x.InicioSessao != null)
                .Where(x => x.FimSessao == null)
                .SingleOrDefault();
        }

    }
}
