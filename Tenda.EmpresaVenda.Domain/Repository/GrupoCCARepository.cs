using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class GrupoCCARepository:NHibernateRepository<GrupoCCA>
    {
        public DataSourceResponse<GrupoCCA> ListarDatatable(DataSourceRequest request, GrupoCCADTO filtro)
        {
            var query = Queryable();

            if (!filtro.Descricao.IsEmpty())
            {
                filtro.Descricao = filtro.Descricao.ToLower();
                query = query.Where(x => x.Descricao.ToLower().Contains(filtro.Descricao));
            }

            return query.ToDataRequest(request);
        }

        public bool ValidarGrupo(long idGrupo, string descricao)
        {
            var query = Queryable();
            query = query.Where(x => x.Id != idGrupo);
            query = query.Where(x => x.Descricao.ToLower().Equals(descricao.ToLower()));
            return query.Any();
        }

        public List<GrupoCCA> ListarTodosCCAs()
        {
            var query = Queryable();

            return query.ToList();
        }
    }
}
