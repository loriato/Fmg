using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewCoordenadorRepository : NHibernateRepository<ViewCoordenador>
    {
        public DataSourceResponse<ViewCoordenador> Listar(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var query = Queryable().Where(x=>x.SituacaoCoordenador==Situacao.Ativo);

            if (filtro.NomeCoordenador.HasValue())
            {
                query = query.Where(x => x.NomeCoordenador.ToLower().Contains(filtro.NomeCoordenador.ToLower()));
            }

            return query.ToDataRequest(request);
        }
    }
}
