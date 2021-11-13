using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class AceitesBannersRepository : NHibernateRepository<ViewAceitesBanners>
    {
        public DataSourceResponse<ViewAceitesBanners> Listar(DataSourceRequest request,long? id)
        {
            var query = Queryable();
            if (id != null)
            {
                query = query.Where(x => x.IdBannerPortalEv == id);
            }
            return query.ToDataRequest(request);
        }
    }
}
