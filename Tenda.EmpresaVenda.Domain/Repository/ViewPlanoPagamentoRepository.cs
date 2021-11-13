using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Commons;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPlanoPagamentoRepository : NHibernateRepository<ViewPlanoPagamento>
    {

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="idPreProposta"></param>
        /// <returns></returns>
        public DataSourceResponse<ViewPlanoPagamento> ListarDetalhamentoFinanceiro(DataSourceRequest request, long idPreProposta)
        {
            return Queryable()
                .Where(reg => TipoParcelaWrapper.TipoParcelaDetalhamentoFinanceiro().Contains(reg.TipoParcela))
                .Where(reg => reg.IdPreProposta == idPreProposta)
                .ToDataRequest(request);
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="idPreProposta"></param>
        /// <returns></returns>
        public DataSourceResponse<ViewPlanoPagamento> ListarItbiEmolumentos(DataSourceRequest request, long idPreProposta)
        {
            return Queryable()
                .Where(reg => TipoParcelaWrapper.TipoParcelaItbiEmolumento().Contains(reg.TipoParcela))
                .Where(reg => reg.IdPreProposta == idPreProposta)
                .ToDataRequest(request);
        }
    }
}
