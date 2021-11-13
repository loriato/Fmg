using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ConsolidadoPontuacaoFidelidadeRepository : NHibernateRepository<ConsolidadoPontuacaoFidelidade>
    {
        public ConsolidadoPontuacaoFidelidade FindByIdProposta(long idProposta)
        {
            return Queryable()
                .Where(x => x.IdProposta == idProposta)
                .SingleOrDefault();
        }

        public List<ConsolidadoPontuacaoFidelidade> ListarFaturados()
        {
            return Queryable()
                .Where(x => x.Faturado)
                .ToList();
        }
    }
}
