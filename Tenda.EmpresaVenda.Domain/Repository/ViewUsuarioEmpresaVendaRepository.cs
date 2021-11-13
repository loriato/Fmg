using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewUsuarioEmpresaVendaRepository:NHibernateRepository<ViewUsuarioEmpresaVenda>
    {
        public IQueryable<ViewUsuarioEmpresaVenda> BuscarEvsPorUsuario(long idUsuario) => Queryable()
                .Where(x => x.IdUsuario == idUsuario);

        public IQueryable<ViewUsuarioEmpresaVenda> ListarAutoCompleteCACT(DataSourceRequest request,long idUsuario)
        {
            var results = BuscarEvsPorUsuario(idUsuario);

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("nomefantasia"))
                    {
                        results = results.Where(x => x.NomeEmpresaVenda.ToLower().Contains(filtro.value.ToLower()));
                    }
                    
                    if (filtro.column.ToLower().Equals("idspermitidos") && filtro.value.HasValue())
                    {
                        var valor = filtro.value.Trim()
                            .Split(',')
                            .Where(x => x.Length > 0)
                            .Select(x => long.Parse(x))
                            .ToList();
                        results = results.Where(x => valor.Contains(x.Id));
                    }
                }
            }

            return results;
        }
    }
}
