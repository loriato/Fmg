using Europa.Data;
using NHibernate;
using System;
using System.Linq;
using System.Text;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class ExecucaoRepository : NHibernateRepository<Execucao>
    {
        public ExecucaoRepository(ISession session) : base(session)
        {
        }

        public void ExcluirExecucaoPorQuartzPorPeriodo(long idQuartz, DateTime dataInicio, DateTime dataFim)
        {
            StringBuilder hql = new StringBuilder();

            hql.Append(" DELETE FROM Execucao exec ");
            hql.Append(" WHERE exec.Quartz.Id = :idQuartz ");
            hql.Append(" AND date_trunc('day', exec.CriadoEm) >= date_trunc('day', :dataInicio) ");
            hql.Append(" AND date_trunc('day', exec.CriadoEm) <= date_trunc('day', :dataFim) ");

            IQuery query = Session.CreateQuery(hql.ToString());
            query.SetParameter("idQuartz", idQuartz);
            query.SetParameter("dataInicio", dataInicio);
            query.SetParameter("dataFim", dataFim);
            query.ExecuteUpdate();
        }

        public DateTime DataUltimaExecucao(string caminhoCompleto)
        {
            return Queryable().Where(x => x.Quartz.CaminhoCompleto == caminhoCompleto)
                .Where(x => x.DataFimExecucao != null)
                .Where(x => x.DataFimExecucao != DateTime.MinValue)
                .OrderByDescending(x => x.DataFimExecucao)
                .Select(x => x.DataFimExecucao)
                .FirstOrDefault();
        }

        public Execucao BuscarExecucaoPorId(long idExecucao)
        {
            return Queryable().SingleOrDefault(x => x.Id == idExecucao);
        }

    }
}
