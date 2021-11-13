using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class StandVendaEmpresaVendaRepository : NHibernateRepository<StandVendaEmpresaVenda>
    {
        public List<StandVendaEmpresaVenda> BuscarPorIdStandVenda(long idStandVenda)
        {
            return Queryable()
                    .Where(x => x.StandVenda.Id == idStandVenda)
                    .ToList();
        }
    }
}
