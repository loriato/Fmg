using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewDocumentoFormularioRepository : NHibernateRepository<ViewDocumentoFormulario>
    {
        public DataSourceResponse<ViewDocumentoFormulario> ListarDocumentos(DataSourceRequest request,DocumentoFormularioDTO filtro)
        {
            var query = Queryable().Where(x => x.IdPreProposta == filtro.IdPreProposta);

            return query.ToDataRequest(request);
        }
    }
}
