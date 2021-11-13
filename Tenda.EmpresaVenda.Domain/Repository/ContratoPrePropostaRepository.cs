using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ContratoPrePropostaRepository : NHibernateRepository<ContratoPreProposta>
    {
        public ContratoPreProposta BuscarContratoMaisRecente(long idPreProposta)
        {
            return Queryable().Where(reg => reg.PreProposta.Id == idPreProposta).OrderByDescending(reg => reg.CriadoEm).FirstOrDefault();
        }
    }
}
