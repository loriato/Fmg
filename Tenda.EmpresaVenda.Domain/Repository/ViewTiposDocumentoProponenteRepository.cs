using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.TiposDocumento;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewTiposDocumentoProponenteRepository : NHibernateRepository<ViewTiposDocumento>
    {
        public DataSourceResponse<ViewTiposDocumento> Listar(FiltroTiposDocumentoDTO filtro)
        {
            var query = Queryable()
                       .Where(reg => reg.Situacao == Situacao.Ativo);

            if (filtro.Nome.HasValue())
            {
                query = query.Where(reg => reg.Nome.Contains(filtro.Nome));
            }

            return query.ToDataRequest(filtro.Request);
        }
    }
}
