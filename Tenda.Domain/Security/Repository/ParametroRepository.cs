using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class ParametroRepository : NHibernateRepository<Parametro>
    {
        public ParametroRepository(ISession session) : base(session) { }

        public IQueryable<Parametro> BuscarParametros(string chave, long? idSistema)
        {
            var query = Queryable();
            if (!chave.IsEmpty())
            {
                query = query.Where(x => x.Chave.ToLower().Contains(chave.ToLower()) ||
                                         x.Valor.ToLower().Contains(chave.ToLower()) ||
                                         x.Descricao.ToLower().Contains(chave.ToLower()));
            }
            if (idSistema.HasValue)
            {
                query = query.Where(x => x.Sistema.Id == idSistema.Value);
            }
            return query;
        }

        public Parametro GetById(long id)
        {
            var result = Queryable().FirstOrDefault(x => x.Id == id);
            return result;
        }

        public bool HasChave(Parametro parametro, long idSistema)
        {
            return Queryable()
                        .Where(reg => reg.Sistema.Id == idSistema)
                        .Where(reg => reg.Chave.ToLower() == parametro.Chave.ToLower())
                        .Where(reg => reg.Id != parametro.Id)
                        .Any();
        }

        public Parametro FindByKey(string key)
        {
            return Queryable()
                .Where(reg => reg.Chave.ToLower() == key.ToLower())
                .FirstOrDefault();
        }

        public Parametro BuscarPorSistemaAndChave(string codigoSistema, string chave)
        {
            return Queryable()
                .Where(reg => reg.Sistema.Codigo.ToUpper() == codigoSistema)
                .Where(reg => reg.Chave.ToUpper() == chave.ToUpper())
                .FirstOrDefault();
        }

        public List<string> BuscarTodasChavesPorPrefixo(string prefix)
        {
            return Queryable()
                .Where(reg => reg.Chave.ToUpper().StartsWith(prefix))
                .Select(reg => reg.Chave)
                .ToList();
        }
    }
}
