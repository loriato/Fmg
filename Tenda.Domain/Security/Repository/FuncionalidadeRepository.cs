using Europa.Data;
using NHibernate;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class FuncionalidadeRepository : NHibernateRepository<Funcionalidade>
    {
        public FuncionalidadeRepository(ISession session) : base(session)
        {
        }

        public bool VerificarUnicidadeNome(Funcionalidade model)
        {
            var existe = Queryable().Where(x => x.UnidadeFuncional.Id == model.UnidadeFuncional.Id)
                                    .Where(x => x.Nome.ToLower().Equals(model.Nome.ToLower()))
                                    .Where(x => x.Id != model.Id ).Any();
            return existe;
        }

        public bool VerificarUnicidadeComando(Funcionalidade model)
        {
            var existe = Queryable().Where(x => x.UnidadeFuncional.Id == model.UnidadeFuncional.Id)
                                    .Where(x => x.Comando.ToLower().Equals(model.Comando.ToLower()))
                                    .Where(x => x.Id != model.Id).Any();
            return existe;
        }

    }
}
