using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.KanbanPreProposta;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class CardKanbanSituacaoPrePropostaRepository : NHibernateRepository<CardKanbanSituacaoPreProposta>
    {
        public List<CardKanbanSituacaoPreProposta> Listar (FiltroKanbanPrePropostaDto filtro)
        {
            var query = Queryable();

            if (filtro.IdCardKanbanPreProposta.HasValue())
            {
                query = query.Where(x => x.CardKanbanPreProposta.Id == filtro.IdCardKanbanPreProposta);
            }

            return query.ToList();
        }
    }
}
