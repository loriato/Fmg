using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class GrupoCCAEmpresaVendaRepository:NHibernateRepository<GrupoCCAEmpresaVenda>
    {
        public List<long> ListarIdsEvsPorGrupo(List<long> idsGrupo)
        {
            return Queryable()
                .Where(x => idsGrupo.Contains(x.GrupoCCA.Id)) 
                .GroupBy(x=>x.EmpresaVenda.Id)
                .Select(x => x.Key)                
                .ToList();
        }

        public List<GrupoCCAEmpresaVenda> BuscarGruposPorEv(long idEmpresaVenda)
        {
            var query = Queryable()
                        .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                        .ToList();
            return query;
        }
    }

}
