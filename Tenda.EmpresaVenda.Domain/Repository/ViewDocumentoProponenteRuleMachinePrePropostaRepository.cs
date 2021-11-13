using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewDocumentoProponenteRuleMachinePrePropostaRepository : NHibernateRepository<ViewDocumentoProponenteRuleMachinePreProposta>
    {
        public DataSourceResponse<ViewDocumentoProponenteRuleMachinePreProposta> ListarDocumentoProponenteRule(DataSourceRequest request, FiltroRuleMachineDTO filtro)
        {
            var query = Queryable().Where(x => x.IdRuleMachinePreProposta == filtro.IdRuleMachine);

            if (filtro.IdTipoDocumento.HasValue())
            {
                query = query.Where(x => x.IdTipoDocumento == filtro.IdTipoDocumento);
            }

            if (filtro.Origem.HasValue())
            {
                query = query.Where(x => x.Origem == filtro.Origem);
            }

            if (filtro.Destino.HasValue())
            {
                query = query.Where(x => x.Destino == filtro.Destino);
            }

            return query.ToDataRequest(request);
        }
    }
}
