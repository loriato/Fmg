using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoFornecedorRepository : NHibernateRepository<EnderecoFornecedor>
    {
        public DataSourceResponse<EnderecoFornecedor> ListarDatatable(DataSourceRequest request,FiltroFornecedorDTO filtro)
        {
            var query = Queryable();

            if (filtro.CodigoFornecedor.HasValue())
            {
                query = query.Where(x => x.CodigoFornecedor.ToUpper().Contains(filtro.CodigoFornecedor.ToUpper()));
            }

            if (filtro.Estados.HasValue() && !filtro.Estados.Contains(""))
            {
                query = query.Where(x => filtro.Estados.Contains(x.Estado));
            }

            return query.ToDataRequest(request);
        }

        public EnderecoFornecedor BuscarEnderecoPorCodigoERegional(string codigo, string regional)
        {
            return Queryable()
                .Where(x => x.CodigoFornecedor.ToUpper().Equals(codigo.ToUpper()))
                .Where(x => x.Estado.ToUpper().Equals(regional.ToUpper()))
                .SingleOrDefault();
        }

    }
}
