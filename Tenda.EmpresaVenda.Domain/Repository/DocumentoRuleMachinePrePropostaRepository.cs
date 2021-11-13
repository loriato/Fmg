using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class DocumentoRuleMachinePrePropostaRepository : NHibernateRepository<DocumentoProponenteRuleMachinePreProposta>
    {
        public List<DocumentoProponenteRuleMachinePreProposta> FindByIdRuleMachine(long idRuleMachine)
        {
            return Queryable()
                .Where(x => x.RuleMachinePreProposta.Id == idRuleMachine)
                .ToList();
        }
    }
}
