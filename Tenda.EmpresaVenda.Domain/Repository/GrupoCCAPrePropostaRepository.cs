using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class GrupoCCAPrePropostaRepository : NHibernateRepository<GrupoCCAPreProposta>
    {
        public List<GrupoCCAPreProposta> FindByPreProposta(long idPreProposta)
        {
            var result = Queryable()
                         .Where(x => x.IdPreProposta == idPreProposta)
                         .Where(x => x.Situacao == Situacao.Ativo)
                         .OrderBy(x => x.CriadoEm)
                         .ToList<GrupoCCAPreProposta>();
            return result;
        }
    }
}
