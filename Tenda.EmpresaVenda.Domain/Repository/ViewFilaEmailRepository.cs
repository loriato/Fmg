using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewFilaEmailRepository:NHibernateRepository<ViewFilaEmail>
    {
        public DataSourceResponse<ViewFilaEmail> ListarFilaEmail(DataSourceRequest request,FiltroEmailDTO filtro)
        {
            var query = Queryable();

            if (filtro.Destinatario.HasValue())
            {
                filtro.Destinatario = filtro.Destinatario.ToUpper();
                query = query.Where(x => x.Destinatario.ToUpper().Contains(filtro.Destinatario));
            }

            if (filtro.Situacoes.HasValue())
            {
                query = query.Where(x => filtro.Situacoes.Contains(x.SituacaoEnvio));
            }

            if (filtro.PeriodoDe.HasValue())
            {
                query = query.Where(x => x.DataEnvio != null)
                    .Where(x => x.DataEnvio.Value.Date >= filtro.PeriodoDe.Value.Date);
            }

            if (filtro.PeriodoAte.HasValue())
            {
                query = query.Where(x => x.DataEnvio != null)
                    .Where(x => x.DataEnvio.Value.Date <= filtro.PeriodoAte.Value.Date);
            }

            return query.ToDataRequest(request);
        }
    }
}
