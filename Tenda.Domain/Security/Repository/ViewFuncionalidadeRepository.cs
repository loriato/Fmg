using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Security.Repository.Models;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewFuncionalidadeRepository : NHibernateStatelessRepository<ViewFuncionalidade>
    {
        public IQueryable<ViewFuncionalidade> Listar (FiltroFuncionalidadeDTO filtro)
        {
            var query = Queryable();
            if (filtro.NomeFuncionalidade.HasValue())
            {
                query = query.Where(x => x.NomeFuncionalidade.ToLower().Contains(filtro.NomeFuncionalidade.ToLower()));
            }
            if (filtro.IdUnidadeFuncional.HasValue())
            {
                query = query.Where(x => x.IdUnidadeFuncional.Equals(filtro.IdUnidadeFuncional));
            }
            if (filtro.IdSistema.HasValue())
            {
                query = query.Where(x => x.IdSistema.Equals(filtro.IdSistema));
            }
            return query;
        }
    }
}
