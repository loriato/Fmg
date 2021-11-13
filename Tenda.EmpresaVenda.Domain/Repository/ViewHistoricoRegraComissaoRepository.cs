using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewHistoricoRegraComissaoRepository:NHibernateRepository<ViewHistoricoRegraComissao>
    {
        public DataSourceResponse<ViewHistoricoRegraComissao>ListarHistorico(DataSourceRequest request, RegraComissaoDTO filtro)
        {
            var query = Queryable();

            if (!filtro.Descricao.IsEmpty())
            {
                filtro.Descricao = filtro.Descricao.ToLower();
                query = query.Where(x => x.Descricao.ToLower().Equals(filtro.Descricao));
            }

            return query.OrderByDescending(x=>x.CriadoEm).ToDataRequest(request);
        }
    }
}
