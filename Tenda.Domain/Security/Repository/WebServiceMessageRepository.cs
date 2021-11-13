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
    public class WebServiceMessageRepository : NHibernateRepository<WebServiceMessage>
    {
        public WebServiceMessageRepository(ISession session) : base(session)
        {
        }

        public int ExcluirLogs(LogWebServiceDTO filtro)
        {
            var hql = new StringBuilder();
            hql.Append(" DELETE FROM WebServiceMessage ws ");
            hql.Append(" WHERE 1 = 1 ");
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (filtro.Endpoint.HasValue())
            {
                hql.Append(" AND ws.Endpoint = :endpoint ");
                parameters.Add("endpoint", filtro.Endpoint);
            }
            if (filtro.Operacao.HasValue())
            {
                hql.Append(" AND ws.Action = :operacao ");
                parameters.Add("operacao", filtro.Operacao);
            }
            if (filtro.Stage.HasValue())
            {
                hql.Append(" AND ws.Stage = :stage ");
                parameters.Add("stage", filtro.Stage);
            }
            if (filtro.Conteudo.HasValue())
            {
                hql.Append(" AND ws.Content LIKE :conteudo ");
                parameters.Add("conteudo", "%" + filtro.Conteudo + "%");
            }
            if (filtro.HorarioInicio.HasValue())
            {
                hql.Append(" AND ws.CriadoEm >= :horarioInicio ");
                parameters.Add("horarioInicio", filtro.HorarioInicio);
            }
            if (filtro.HorarioFim.HasValue())
            {
                hql.Append(" AND ws.CriadoEm <= :horarioFim ");
                parameters.Add("horarioFim", filtro.HorarioFim);
            }

            var query = Session.CreateQuery(hql.ToString());

            foreach (var param in parameters)
            {
                query.SetParameter(param.Key, param.Value);
            }

            return query.ExecuteUpdate();
        }

        public DataSourceResponse<WebServiceMessage> Listar(DataSourceRequest request, LogWebServiceDTO filtro)
        {
            var data = Queryable();

            if (!filtro.Endpoint.IsEmpty())
            {
                data = data.Where(x => x.Endpoint == filtro.Endpoint);
            }
            if (!filtro.Operacao.IsEmpty())
            {
                data = data.Where(x => x.Action == filtro.Operacao);
            }
            if (!filtro.Conteudo.IsEmpty())
            {
                data = data.Where(x => x.Content.Contains(filtro.Conteudo));
            }
            if (!filtro.HorarioInicio.IsEmpty())
            {
                data = data.Where(x => x.CriadoEm >= filtro.HorarioInicio);
            }
            if (!filtro.HorarioFim.IsEmpty())
            {
                data = data.Where(x => x.CriadoEm <= filtro.HorarioFim);
            }
            if (!filtro.Stage.IsEmpty())
            {
                data = data.Where(x => x.Stage == filtro.Stage);
            }

            return data.ToDataRequest(request);
        }
    }
}
