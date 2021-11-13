using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewStandVendaEmpresaVendaRepository : NHibernateRepository<ViewStandVendaEmpresaVenda>
    {
        public DataSourceResponse<ViewStandVendaEmpresaVenda> ListarDatatableStandVendaEmpresaVenda(DataSourceRequest request, FiltroStandVendaDTO filtro)
        {
            
            var query = Queryable().Where(x => x.IdStandVenda == filtro.IdStandVenda); 

            if (filtro.Nome.HasValue())
            {
                query = query.Where(x => x.NomeEmpresaVenda.ToUpper().Contains(filtro.Nome.ToUpper()));
            }

            if (filtro.IdRegional.HasValue())
            {
                query = query.Where(x => x.IdRegional.Contains('#'+filtro.IdRegional.ToString()+'#'));
            }
            if (filtro.Estado.HasValue())
            {
                query = query.Where(x => x.Estado.ToUpper().Contains(filtro.Estado.ToUpper()));
            }
            
            return query.ToDataRequest(request);
        }
    }
}
