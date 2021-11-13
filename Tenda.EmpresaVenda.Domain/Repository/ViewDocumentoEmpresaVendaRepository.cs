using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewDocumentoEmpresaVendaRepository : NHibernateRepository<ViewDocumentoEmpresaVenda>
    {
        public DataSourceResponse<ViewDocumentoEmpresaVenda> ListarDocumentosEmpresaVenda(DataSourceRequest request,FiltroDocumentoEmpresaVendaDto filtro)
        {
            var query = Queryable();

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(x => filtro.IdEmpresaVenda == x.IdEmpresaVenda);
            }

            return query.ToDataRequest(request);
        }
    }
}
