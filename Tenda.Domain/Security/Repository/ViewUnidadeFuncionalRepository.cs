using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewUnidadeFuncionalRepository : NHibernateRepository<ViewUnidadeFuncional>
    {
        public DataSourceResponse<ViewUnidadeFuncional> Listar(DataSourceRequest request, string codigo, string nome, string endereco, long[] hierarquia, int[] situacao, ExibirMenuFiltro exibirmenu)
        {
            var query = Queryable();
            if (codigo.HasValue())
            {
                query = query.Where(x => x.Codigo.ToLower().Contains(codigo.ToLower()));
            }
            if (nome.HasValue())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));
            }
            if (!situacao.IsEmpty())
            {
                query = query.Where(x => situacao.Contains((int)x.Situacao));
            }
            if (exibirmenu != 0)
            {
                var menu = ((int)exibirmenu == 2 ? 0 : 1); 
                query = query.Where(x => (int)x.ExibirMenu == menu);
            }
            if (endereco.HasValue())
            {
                query = query.Where(x => x.Endereco.ToLower().Contains(endereco.ToLower()));
            }
            if (!hierarquia.IsNull())
            {
                query = query.Where(x => hierarquia.Contains(x.IdHierarquiaModulo));
            }
            return query.ToDataRequest(request);
        }

    }
}
