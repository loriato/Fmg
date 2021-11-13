using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoAvalistaRepository:NHibernateRepository<EnderecoAvalista>
    {
        public EnderecoAvalista FindByIdAvalista(long idAvalista)
        {
            var query = Queryable();
            query = query.Where(x => x.Avalista.Id == idAvalista);
            return query.FirstOrDefault();
        }
    }
}
