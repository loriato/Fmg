using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewAvalistaRepository : NHibernateRepository<ViewAvalista>
    {
        public DataSourceResponse<ViewAvalista> Listar(DataSourceRequest request, EstadoAvalistaDTO filtro)
        {
            var query = Queryable(); 
            if (filtro.Estado.HasValue())
            {
                query = query.Where(x => x.NomeEstado.ToUpper() == filtro.Estado.ToUpper());
            }
            else
            {
                query = query.Where(x => x.Id < 0);
            }

            if (filtro.NomeAvalista.HasValue())
            {
                query = query.Where(x => x.NomeAvalista.ToUpper().Contains(filtro.NomeAvalista.ToUpper()));
            }
            
            return query.ToDataRequest(request);
        }
        public DataSourceResponse<ViewAvalista> ListarTodosAvalistas(DataSourceRequest request)
        {
            return Queryable().ToDataRequest(request);
        }

    }
}
