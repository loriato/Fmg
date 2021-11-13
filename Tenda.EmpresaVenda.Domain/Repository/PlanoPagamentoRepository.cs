using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Commons;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PlanoPagamentoRepository : NHibernateRepository<PlanoPagamento>
    {
        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="idPreProposta"></param>
        /// <returns></returns>
        public DataSourceResponse<PlanoPagamento> ListarDetalhamentoFinanceiro(DataSourceRequest request, long idPreProposta)
        {
            return Queryable()
                .Where(reg => TipoParcelaWrapper.TipoParcelaDetalhamentoFinanceiro().Contains(reg.TipoParcela))
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .ToDataRequest(request);
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="idPreProposta"></param>
        /// <returns></returns>
        public DataSourceResponse<PlanoPagamento> ListarItbiEmolumentos(DataSourceRequest request, long idPreProposta)
        {
            return Queryable()
                .Where(reg => TipoParcelaWrapper.TipoParcelaItbiEmolumento().Contains(reg.TipoParcela))
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .ToDataRequest(request);
        }

        public decimal SomatorioDetalhamentoFinanceiro(long idPreProposta)
        {
            var possuiParcela = Queryable()
              .Where(reg => TipoParcelaWrapper.TipoParcelaDetalhamentoFinanceiro().Contains(reg.TipoParcela))
              .Where(reg => reg.PreProposta.Id == idPreProposta).Any();

            // Não tem um 'SumOrDefault'
            if (!possuiParcela) { return 0; }

            return Queryable()
                .Where(reg => TipoParcelaWrapper.TipoParcelaDetalhamentoFinanceiro().Contains(reg.TipoParcela))
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Sum(reg => reg.Total);
        }

        public decimal SomatorioItbiEmolumentos(long idPreProposta)
        {
            var possuiParcela = Queryable()
               .Where(reg => TipoParcelaWrapper.TipoParcelaItbiEmolumento().Contains(reg.TipoParcela))
               .Where(reg => reg.PreProposta.Id == idPreProposta).Any();

            // Não tem um 'SumOrDefault'
            if (!possuiParcela) { return 0; }

            return Queryable()
                .Where(reg => TipoParcelaWrapper.TipoParcelaItbiEmolumento().Contains(reg.TipoParcela))
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Sum(reg => reg.Total);
        }

        public decimal SomatorioTotal(long idPreProposta)
        {
            var possuiParcela = Queryable()
               .Where(reg => reg.PreProposta.Id == idPreProposta).Any();

            // Não tem um 'SumOrDefault'
            if (!possuiParcela) { return 0; }

            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Sum(reg => reg.Total);
        }

        public List<PlanoPagamento> ListarParcelas(long idPreProposta)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .ToList();
        }

        public bool RegraAvalistaPreChaves(long idPreProposta)
        {
            return Queryable().Where(x => x.PreProposta.Id == idPreProposta)
                              .Where(x => x.TipoParcela == TipoParcela.PreChaves)
                              .Where(x => x.Total < 1000).Any();
        }
        public bool RegraAvalistaPosChaves(long idPreProposta)
        {
            return Queryable().Where(x => x.PreProposta.Id == idPreProposta)
                              .Where(x => x.TipoParcela == TipoParcela.PosChaves).Any();
        }
    }
}
