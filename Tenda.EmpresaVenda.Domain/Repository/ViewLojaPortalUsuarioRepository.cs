using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewLojaPortalUsuarioRepository : NHibernateRepository<ViewLojaPortalUsuario>
    {
        public DataSourceResponse<ViewLojaPortalUsuario> Listar(FiltroAgenteVendaDto filtro)
        {
            var query = Queryable();

            if (filtro.NomeUsuario.HasValue())
            {
                query = query.Where(x => x.NomeUsuario.ToLower().Contains(filtro.NomeUsuario.ToLower()));
            }
            if (filtro.IdLojaPortal.HasValue())
            {
                query = query.Where(x => x.IdLojaPortal == filtro.IdLojaPortal);
            }

            var dataSource = query.ToDataRequest(filtro.DataSourceRequest);
            return dataSource;
        }
    }
}
