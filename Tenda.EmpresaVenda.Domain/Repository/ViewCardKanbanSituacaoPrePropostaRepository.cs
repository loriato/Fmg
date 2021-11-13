using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewCardKanbanSituacaoPrePropostaRepository : NHibernateRepository<ViewCardKanbanSituacaoPreProposta>
    {
        public DataSourceResponse<ViewCardKanbanSituacaoPreProposta> ListarSituacaoCardKanban(DataSourceRequest request,FiltroKanbanPrePropostaDto filtro)
        {
            var query = Queryable();

            if (filtro.IdAreaKanbanPreProposta.HasValue())
            {
                query = query.Where(x => x.IdAreaKanbanPreProposta == filtro.IdAreaKanbanPreProposta);
            }

            if (filtro.IdCardKanbanPreProposta.HasValue())
            {
                query = query.Where(x => x.IdCardKanbanPreProposta == filtro.IdCardKanbanPreProposta);
            }

            return query.ToDataRequest(request);
        }
    }
}
