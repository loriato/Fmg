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
    public class CentroCustoRepository : NHibernateRepository<CentroCusto>
    {
        public IQueryable<CentroCusto> Listar()
        {
            return Queryable();
        }

        public bool CheckIfExistsCodigo(CentroCusto model)
        {
            return Queryable().Where(pntv => pntv.Id != model.Id)
                              .Where(pntv => pntv.Codigo.Equals(model.Codigo))
                              .Any();
        }

        public IQueryable<CentroCusto> ListarAutoComplete(DataSourceRequest request)
        {
            var results = Queryable();

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("codigo"))
                    {
                        results = results.Where(x => x.Codigo.ToString().ToLower().Contains(filtro.value.ToLower()));
                    }

                }
            }
            return results;
        }
    }
}
