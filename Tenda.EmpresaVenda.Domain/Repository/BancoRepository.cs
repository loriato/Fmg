using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class BancoRepository : NHibernateRepository<Banco>
    {

        public IQueryable<Banco> Listar()
        {
            return Queryable();
        }
    }
}
