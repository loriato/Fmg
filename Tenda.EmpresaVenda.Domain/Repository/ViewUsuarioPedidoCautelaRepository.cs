using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Fmg.Models.Views;

namespace Europa.Fmg.Domain.Repository
{
    public class ViewUsuarioPedidoCautelaRepository : NHibernateRepository<ViewUsuarioPedidoCautela>
    {
        public DataSourceResponse<ViewUsuarioPedidoCautela> Listar(DataSourceRequest request, ViewUsuarioPedidoCautela filtro)
        {
            var query = Queryable();
            if (filtro.IdUsuario.HasValue())
            {
                query = query.Where(x => x.IdUsuario == filtro.IdUsuario);
            }
            return query.ToDataRequest(request);
        }
    }
}
