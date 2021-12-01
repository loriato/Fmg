using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Fmg.Models.Views;

namespace Europa.Fmg.Domain.Repository
{
    public class ViewViaturaUsuarioRepository : NHibernateRepository<ViewViaturaUsuario>
    {
        public DataSourceResponse<ViewViaturaUsuario> Listar(DataSourceRequest request, ViewViaturaUsuario filtro)
        {
            var query = Queryable();
            if (filtro.IdUsuario.HasValue())
            {
                query = query.Where(x => x.IdUsuario == filtro.IdUsuario);
            }
            return query.ToDataRequest(request);
        }

        public DataSourceResponse<ViewViaturaUsuario> ListarViaturaEmPercuso(DataSourceRequest request)
        {
            var query = Queryable().Where(x => x.DataEntrega == null);

            return query.ToDataRequest(request);
        }
    }
}
