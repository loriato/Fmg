using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoCorretorRepository : NHibernateRepository<EnderecoCorretor>
    {

        public EnderecoCorretor FindByCorretor(long idCorretor)
        {
            return Queryable().Where(x => x.Corretor.Id == idCorretor).FirstOrDefault();
        }
    }
}
