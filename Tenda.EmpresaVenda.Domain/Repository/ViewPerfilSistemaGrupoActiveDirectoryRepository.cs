using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPerfilSistemaGrupoActiveDirectoryRepository : NHibernateRepository<ViewPerfilSistemaGrupoActiveDirectory>
    {
        public ViewPerfilSistemaGrupoActiveDirectoryRepository(ISession session) : base(session)
        {
        }

        public DataSourceResponse<ViewPerfilSistemaGrupoActiveDirectory> Listar(DataSourceRequest request, long idSistema)
        {
            var query = Queryable();

            query = query.Where(reg => reg.IdSistema == idSistema);

            return query.ToDataRequest(request);
        }
    }
}
