using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System;
using System.Linq;
using System.Text;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class LogExecucaoRepository : NHibernateRepository<LogExecucao>
    {
        public LogExecucaoRepository(ISession session) : base(session)
        {
        }

        public void ExcluirLogsExecucaoPorQuartzPorPeriodo(long idQuartz, DateTime dataInicio, DateTime dataFim)
        {
            StringBuilder hql = new StringBuilder();

            hql.Append(" DELETE FROM LogExecucao loex WHERE ");
            hql.Append(" EXISTS (SELECT 1 FROM Execucao exec  ");
            hql.Append("    WHERE exec.Quartz.Id = :idQuartz AND loex.Execucao.Id = exec.Id ");
            hql.Append("    AND date_trunc('day', exec.CriadoEm) >= date_trunc('day', :dataInicio) ");
            hql.Append("    AND date_trunc('day', exec.CriadoEm) <= date_trunc('day', :dataFim)) ");

            IQuery query = Session.CreateQuery(hql.ToString());
            query.SetParameter("idQuartz", idQuartz);
            query.SetParameter("dataInicio", dataInicio);
            query.SetParameter("dataFim", dataFim);
            query.ExecuteUpdate();
        }

        public DataSourceResponse<LogExecucao> ListarLogsExecucao(DataSourceRequest request, DateTime? dataInicio, DateTime? dataFim, TipoLog? tipo, string log, long idExecucao)
        {
            var results = BuscarLogsPorIdExecucao(idExecucao);

            if (!dataInicio.IsEmpty())
            {
                results = results.Where(x => x.CriadoEm.Date >= dataInicio);
            }
            if (!dataFim.IsEmpty())
            {
                results = results.Where(x => x.CriadoEm.Date <= dataFim);
            }
            if (!tipo.IsEmpty())
            {
                results = results.Where(x => x.Tipo == tipo);
            }
            if (!log.IsEmpty())
            {
                results = results.Where(x => x.Log.ToLower().Contains(log.ToLower()));
            }
            return results.ToDataRequest(request);
        }

        public IQueryable<LogExecucao> BuscarLogsPorIdExecucao(long idExecucao)
        {
            return Queryable().Where(x => x.Execucao.Id == idExecucao);
        }

        public Execucao BuscarExecucaoPorIdExecucao(long idExecucao)
        {
            if (Queryable().Where(reg => reg.Execucao.Id == idExecucao).Any())
            {
                return Queryable().FirstOrDefault(x => x.Execucao.Id == idExecucao).Execucao;
            }
            else
            {
                return new Execucao();
            }
        }

        public Execucao BuscarUltimaExecucaoSucesso(string caminhoCompleto)
        {
            var query = Queryable()
                .Where(x => x.Execucao.Quartz.CaminhoCompleto == caminhoCompleto)
                .Where(x => x.Execucao.DataFimExecucao != null)
                .Where(x => x.Execucao.DataFimExecucao != DateTime.MinValue);
            
            var idsExecucoesComErro = query.Where(x => x.Tipo == TipoLog.Erro).Select(x => x.Execucao.Id);
            
            return query.Where(x => !idsExecucoesComErro.Contains(x.Execucao.Id))
                .Select(x => x.Execucao)
                .OrderByDescending(x => x.DataFimExecucao)
                .FirstOrDefault();
        }
    }
}
