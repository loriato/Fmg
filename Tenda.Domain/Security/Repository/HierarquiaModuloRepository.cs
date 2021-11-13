using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Linq;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class HierarquiaModuloRepository : NHibernateRepository<HierarquiaModulo>
    {
        public HierarquiaModuloRepository(ISession session) : base(session)
        {

        }

        public IQueryable<HierarquiaModulo> Listar(string nome, long[] idsSistemas, long? moduloPai, Situacao? situacao)
        {
            var result = Queryable();
            if (nome.HasValue())
            {
                result = result.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));
            }
            if (idsSistemas.HasValue())
            {
                result = result.Where(x => idsSistemas.Contains(x.Sistema.Id));
            }
            if (moduloPai.HasValue())
            {
                result = result.Where(x => x.Pai.Id == moduloPai);
            }
            if (situacao.HasValue())
            {
                result = result.Where(x => x.Situacao == situacao);
            }

            return result;
        }

    }
}
