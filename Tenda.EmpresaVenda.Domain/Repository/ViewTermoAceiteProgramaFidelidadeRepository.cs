using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewTermoAceiteProgramaFidelidadeRepository : NHibernateRepository<ViewTermoAceiteProgramaFidelidade>
    {
        public IQueryable<ViewTermoAceiteProgramaFidelidade> Listar()
        {
            return Queryable();
        }
    }
}
