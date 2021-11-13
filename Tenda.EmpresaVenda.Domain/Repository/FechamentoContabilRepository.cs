using Europa.Data;
using Europa.Extensions;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class FechamentoContabilRepository : NHibernateRepository<FechamentoContabil>
    {
        public DataSourceResponse<FechamentoContabil> ListarFechamentoContabil(DataSourceRequest request, FiltroFechamentoContabilDto filtro)
        {
            var query = Queryable();

            if (filtro.InicioFechamento.HasValue())
            {
                query = query.Where(x => x.InicioFechamento.Date >= filtro.InicioFechamento.Date);
            }

            if (filtro.TerminoFechamento.HasValue())
            {
                query = query.Where(x => x.TerminoFechamento.Date <= filtro.TerminoFechamento.Date);
            }

            return query.ToDataRequest(request);
        }
        public List<FechamentoContabil> ListFechamentosANotificar()
        {
            var query = Queryable()
                .Where(x => x.QuantidadeDiasLembrete != 0)
                .Where(x => x.InicioFechamento.Day - x.QuantidadeDiasLembrete == DateTime.Today.Day)
                .Where(x => x.InicioFechamento.Month == DateTime.Today.Month)
                .Where(x => x.InicioFechamento.Year == DateTime.Today.Year);
            return query.ToList();


        }

        public bool CheckIntersecaoData(DateTime data)
        {
            var query = Queryable()
                .Where(x => data.Date >= x.InicioFechamento.Date)
                .Where(x => data.Date <= x.TerminoFechamento.Date)
                .Any();
            return !query;
        }

        public bool CheckPeriodoValido(DateTime inicio,DateTime termino)
        {
            var query = Queryable()
                .Where(x => inicio.Date <= x.InicioFechamento.Date)
                .Where(x => termino.Date >= x.TerminoFechamento.Date)
                .Any();

            return !query;
        }

        public FechamentoContabil FechamentoContabilVigente()
        {
            return Queryable()
                .Where(x => x.InicioFechamento.Date <= DateTime.Now.Date)
                .Where(x => x.TerminoFechamento.Date >= DateTime.Now.Date)
                .SingleOrDefault();
        }
    }
}
