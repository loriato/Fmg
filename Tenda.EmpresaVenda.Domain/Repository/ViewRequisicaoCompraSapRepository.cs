using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewRequisicaoCompraSapRepository : NHibernateRepository<ViewRequisicaoCompraSap>
    {
        public IQueryable<ViewRequisicaoCompraSap> Listar(FiltroRequisicaoCompraSapDTO filtro)
        {
            var query = Queryable();

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.IdProposta.HasValue())
            {
                query = query.Where(x => x.IdProposta == filtro.IdProposta);
            }

            if (filtro.TipoPagamento.HasValue())
            {
                query = query.Where(x => x.TipoPagamento == filtro.TipoPagamento);
            }

            return query;
        }
    }
}
