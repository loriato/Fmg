using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Services.Models;

namespace Tenda.Domain.Security.Repository
{
    public class ErrorRepository : NHibernateRepository<Error>
    {
        public ErrorRepository(ISession session) : base(session)
        {
        }

        public int ExcluirLogs(LogErroDTO filtro)
        {
            var hql = new StringBuilder();
            hql.Append(" DELETE FROM Error er ");
            hql.Append(" WHERE 1 = 1 ");
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (filtro.Type.HasValue())
            {
                hql.Append(" AND LOWER(er.Type) LIKE :type ");
                parameters.Add("type", "%" + filtro.Type.ToLower() + "%");
            }
            if (filtro.Message.HasValue())
            {
                hql.Append(" AND LOWER(er.Message) LIKE :message ");
                parameters.Add("message", "%" + filtro.Message.ToLower() + "%");
            }
            if (filtro.Source.HasValue())
            {
                hql.Append(" AND LOWER(er.Source) LIKE :source ");
                parameters.Add("source", "%" + filtro.Source.ToLower() + "%");
            }
            if (filtro.Stacktrace.HasValue())
            {
                hql.Append(" AND LOWER(er.Stacktrace) LIKE :stacktrace ");
                parameters.Add("stacktrace", "%" + filtro.Stacktrace.ToLower() + "%");
            }
            if (filtro.HorarioInicio.HasValue())
            {
                hql.Append(" AND er.CriadoEm >= :horarioInicio ");
                parameters.Add("horarioInicio", filtro.HorarioInicio);
            }
            if (filtro.HorarioFim.HasValue())
            {
                hql.Append(" AND er.CriadoEm <= :horarioFim ");
                parameters.Add("horarioFim", filtro.HorarioFim);
            }

            var query = Session.CreateQuery(hql.ToString());

            foreach (var param in parameters)
            {
                query.SetParameter(param.Key, param.Value);
            }

            return query.ExecuteUpdate();
        }

        public DataSourceResponse<Error> Listar(DataSourceRequest request, LogErroDTO filtro)
        {
            var data = Queryable();

            if (!filtro.Type.IsEmpty())
            {
                data = data.Where(x => x.Type.ToLower().Contains(filtro.Type.ToLower()));
            }
            if (!filtro.Message.IsEmpty())
            {
                data = data.Where(x => x.Message.ToLower().Contains(filtro.Message.ToLower()));
            }
            if (!filtro.Source.IsEmpty())
            {
                data = data.Where(x => x.Source.Contains(filtro.Source));
            }
            if (!filtro.Stacktrace.IsEmpty())
            {
                data = data.Where(x => x.Stacktrace.ToLower().Contains(filtro.Stacktrace.ToLower()));
            }
            if (!filtro.HorarioInicio.IsEmpty())
            {
                data = data.Where(x => x.CriadoEm >= filtro.HorarioInicio);
            }
            if (!filtro.HorarioFim.IsEmpty())
            {
                data = data.Where(x => x.CriadoEm <= filtro.HorarioFim);
            }

            return data.ToDataRequest(request);
        }
    }
}
