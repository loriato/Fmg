using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class RegionalEmpresaRepository : NHibernateRepository<RegionalEmpresa>
    {
        public IQueryable<RegionalEmpresa> ListarRegionaisPorEmpresa(long idEmpresaVenda)
        {
            var ret = Queryable();
            if (!idEmpresaVenda.IsEmpty() || idEmpresaVenda == 0)
                ret = ret.Where(w=>w.EmpresaVenda.Id==idEmpresaVenda);
            return ret;
        }
        public RegionalEmpresa ListarRegionalComEmpresa(long idEmpresaVenda,long idRegional)
        {
            var ret = Queryable();
            if (idEmpresaVenda.HasValue()) 
                ret = ret.Where(w => w.EmpresaVenda.Id == idEmpresaVenda);
            if (idRegional.HasValue())
                ret = ret.Where(w => w.Regional.Id == idRegional);
            return ret.FirstOrDefault();
        }

    }
}
