using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PontuacaoFidelidadeEmpresaVendaRepository : NHibernateRepository<PontuacaoFidelidadeEmpresaVenda>
    {
        public PontuacaoFidelidadeEmpresaVenda FindByIdEmpresaVenda(long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .SingleOrDefault();
        }
    }
}
