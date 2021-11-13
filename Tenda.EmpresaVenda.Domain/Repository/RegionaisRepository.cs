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
    public class RegionaisRepository : NHibernateRepository<Regionais>
    {
        public Regionais findByName(string name)
        {
            var reg = Queryable().Where(x => x.Nome == name).SingleOrDefault();
            return reg;
        }

        public Regionais findById(long Id)
        {
            var reg = Queryable().Where(x => x.Id == Id).SingleOrDefault();
            return reg;
        }
        public IQueryable<Regionais> Listar(DataSourceRequest request)
        {
            var reg = Queryable();
            foreach (var filtro in request.filter)
            {
                if (filtro.column == "Nome" && filtro.value.HasValue())
                {
                    reg = reg.Where(w => w.Nome.ToLower().Contains(filtro.value.ToLower()));
                }
            }
            return reg;
        }

        public List<Regionais> getAll()
        {
            var reg = Queryable().ToList();
            return reg;
        }

    }
}
