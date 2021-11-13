using Europa.Data;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class UnidadeFuncionalRepository : NHibernateRepository<UnidadeFuncional>
    {
        public UnidadeFuncionalRepository(ISession session) : base(session)
        {
        }

        public IDictionary<string, string> ListarCodigoNomeUnidadesFuncionaisDoSistema(string codigoSistema)
        {
            return Queryable()
                .Where(uf => uf.Modulo.Sistema.Codigo.ToLower().Equals(codigoSistema.ToLower()))
                .OrderBy(uf => uf.Codigo)
                .ToDictionary(reg => reg.Codigo.ToUpper(), reg => reg.Nome);
        }
    }
}
