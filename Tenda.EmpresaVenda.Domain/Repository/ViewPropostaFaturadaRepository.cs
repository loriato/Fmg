using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPropostaFaturadaRepository:NHibernateRepository<ViewPropostaFaturada>
    {
        public DataSourceResponse<ViewPropostaFaturada>Listar(DataSourceRequest request,FiltroPropostaDTO filtro)
        {
            var query = Queryable();

            if (filtro.Faturado.HasValue())
            {
                var faturado = filtro.Faturado == 1;
                query = query.Where(x => x.Faturado == faturado);
            }

            if (filtro.DataFaturadoDe.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date >= filtro.DataFaturadoDe.Value.Date);
            }

            if (filtro.DataFaturadoAte.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date <= filtro.DataFaturadoAte.Value.Date);
            }

            if (filtro.CodigoProposta.HasValue())
            {
                query = query.Where(x => x.CodigoProposta.ToUpper().Contains(filtro.CodigoProposta.ToUpper()));
            }
            if (filtro.Estado.HasValue())
            {
                query = query.Where(x => x.Regional.Equals(filtro.Estado));
            }

            if (filtro.DataVendaDe.HasValue())
            {
                query = query.Where(x => x.DataVenda.Value.Date >= filtro.DataVendaDe.Value.Date);
            }
            if (filtro.DataVendaAte.HasValue())
            {
                query = query.Where(x => x.DataVenda.Value.Date <= filtro.DataVendaAte.Value.Date);
            }

            return query.ToDataRequest(request);
        }
    }
}
