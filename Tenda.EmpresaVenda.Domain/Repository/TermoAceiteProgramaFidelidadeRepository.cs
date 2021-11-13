using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class TermoAceiteProgramaFidelidadeRepository : NHibernateRepository<TermoAceiteProgramaFidelidade>
    {
        public TermoAceiteProgramaFidelidade BuscarUltimoTermoAceite()
        {
            return Queryable().OrderByDescending(x => x.CriadoEm).FirstOrDefault();
        }

        public IQueryable<TermoAceiteProgramaFidelidade> Listar()
        {
            return Queryable();
        }
    }
}
