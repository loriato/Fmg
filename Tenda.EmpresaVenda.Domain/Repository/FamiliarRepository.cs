using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class FamiliarRepository : NHibernateRepository<Familiar>
    {


        public Familiar BuscarConjugePorCliente(long idCliente)
        {
            return Queryable()
                .Where(reg => reg.Familiaridade == TipoFamiliaridade.Conjuge)
                .Where(reg => reg.Cliente1.Id == idCliente || reg.Cliente2.Id == idCliente)
                .FirstOrDefault();
        }
    }
}
