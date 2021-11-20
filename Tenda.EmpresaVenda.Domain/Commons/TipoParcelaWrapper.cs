using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Europa.Fmg.Domain.Commons
{
    public static class TipoParcelaWrapper
    {
        private readonly static List<TipoParcela> _domain = new List<TipoParcela>
        {
            TipoParcela.Ano,
            TipoParcela.Ato,
            TipoParcela.Bimestral,
            TipoParcela.FGTS,
            TipoParcela.Financiamento,
            TipoParcela.Mensal,
            TipoParcela.PosChaves,
            TipoParcela.PosChavesItbi,
            TipoParcela.PreChaves,
            TipoParcela.PreChavesIntermediaria,
            TipoParcela.PreChavesIntermediariaItbi,
            TipoParcela.PreChavesItbi,
            TipoParcela.Repasse,
            TipoParcela.Semestral,
            TipoParcela.Subsidio,
            TipoParcela.PremiadaTenda
        };

        /// <summary>
        /// Todas as parcelas, exceto as que são referentes a ITBI
        /// </summary>
        /// <returns></returns>
        public static List<TipoParcela> TipoParcelaDetalhamentoFinanceiro()
        {
            var parcelasItbi = new List<TipoParcela> {
                TipoParcela.Ato,
                TipoParcela.PreChaves,
                TipoParcela.PreChavesIntermediaria,
                TipoParcela.PosChaves,
                TipoParcela.Financiamento,
                TipoParcela.Subsidio,
                TipoParcela.FGTS,
                TipoParcela.PremiadaTenda
            };

            return _domain.Where(reg => parcelasItbi.Contains(reg)).ToList();
        }

        public static List<TipoParcela> TipoParcelaItbiEmolumento()
        {
            var parcelasItbi = new List<TipoParcela> {
                TipoParcela.PosChavesItbi,
                TipoParcela.PreChavesItbi,
                TipoParcela.PreChavesIntermediariaItbi
            };

            return _domain.Where(reg => parcelasItbi.Contains(reg)).ToList();
        }
    }
}