using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewAceiteRegraComissaoRepository : NHibernateRepository<ViewAceiteRegraComissao>
    {
        public IQueryable<ViewAceiteRegraComissao> ListarPorRegraComissao(long idRegra)
        {
            var result = Queryable();
            if (idRegra.HasValue())
            {
                result = result.Where(reg => reg.IdRegraComissao == idRegra);
            }

            return result;
        }

    }
}
