using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System;
using System.Linq;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewLogExecucaoRepository : NHibernateRepository<ViewLogExecucao>
    {
        public ViewLogExecucaoRepository(ISession session) : base(session)
        {
        }

        public IQueryable<ViewLogExecucao> ListarExecucoesQuartzPorPeriodo(DateTime? dataInicio, DateTime? dataFim, long idQuartz)
        {
            var results = Queryable().Where(x => x.IdQuartzConfiguration == idQuartz);
            if (!dataInicio.IsEmpty())
            {
                results = results.Where(x => x.DataInicioExecucao > dataInicio);
            }
            if (!dataFim.IsEmpty())
            {
                results = results.Where(x => x.DataInicioExecucao < dataFim);
            }
            return results;
        }

    }
}
