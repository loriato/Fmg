using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class AceiteTermoAceiteProgramaFidelidadeRepository : NHibernateRepository<AceiteTermoAceiteProgramaFidelidade>
    {
        public bool VerificarEVAceite(long idEmpresaVenda, long idTermoAceite)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                                .Where(x => x.TermoAceiteProgramaFidelidade.Id == idTermoAceite)
                                .Any();
        }
    }
}
