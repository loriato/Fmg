using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class RuleMachinePrePropostaRepository : NHibernateRepository<RuleMachinePreProposta>
    {
        public DataSourceResponse<RuleMachinePreProposta> ListarRuleMachinePreProposta(DataSourceRequest request,FiltroRuleMachineDTO filtro)
        {
            var query = Queryable();

            if (filtro.IdRuleMachine.HasValue())
            {
                query = query.Where(x => x.Id == filtro.IdRuleMachine);
            }

            if (filtro.Origem.HasValue())
            {
                query = query.Where(x => x.Origem == filtro.Origem);
            }

            if (filtro.Destino.HasValue())
            {
                query = query.Where(x => x.Destino == filtro.Destino);
            }

            return query.ToDataRequest(request);
        }
        public bool TransicaoExistente(RuleMachinePreProposta rule)
        {
            var query = Queryable();

            if (rule.Id.HasValue())
            {
                query = query.Where(x => x.Id != rule.Id);
            }


            query = query.Where(x => x.Origem == rule.Origem)
                .Where(x => x.Destino == rule.Destino);

            return query.Any();
        }
    }
}
