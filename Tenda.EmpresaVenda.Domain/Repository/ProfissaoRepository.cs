using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ProfissaoRepository : NHibernateRepository<Profissao>
    {

        public DataSourceResponse<Profissao> ListarProfissoes(DataSourceRequest request)
        {
            var results = Queryable();
            if (request.filter.FirstOrDefault() != null)
            {
                string filtro = request.filter.FirstOrDefault().column.ToLower();
                string queryTerm = request.filter.FirstOrDefault().value.ToLower();
                if (filtro.Equals("nome"))
                {
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
            }
            return results.ToDataRequest(request);
        }
    }
}
