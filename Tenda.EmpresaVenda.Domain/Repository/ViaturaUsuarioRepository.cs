using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Repository
{
    public class ViaturaUsuarioRepository : NHibernateRepository<ViaturaUsuario>
    {
        public DataSourceResponse<ViaturaUsuario> Listar(DataSourceRequest request, ViaturaUsuario filtro)
        {
            var query = Queryable();
            if (filtro.Id.HasValue())
            {
                query = query.Where(x => x.Viatura.Id == filtro.Id);
            }
            return query.ToDataRequest(request);
        }
        public ViaturaUsuario BuscarEmAberto(long idViatura)
        {
            return Queryable().Where(x => x.Viatura.Id == idViatura)
                .Where(x => x.Entrega == null)
                .FirstOrDefault();
        }
    }
}
