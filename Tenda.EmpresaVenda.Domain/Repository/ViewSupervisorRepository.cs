using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewSupervisorRepository : NHibernateRepository<ViewSupervisor>
    {
        public DataSourceResponse<ViewSupervisor> Listar(DataSourceRequest request , SupervisorViabilizadorDTO filtro)
        {
            var query = Queryable().Where(x=>x.SituacaoSupervisor==Situacao.Ativo);
            if (filtro.NomeSupervisor.HasValue())
            {
                query = query.Where(x => x.NomeSupervisor.ToUpper().Contains(filtro.NomeSupervisor.ToUpper()));
            }
            return query.ToDataRequest(request);
        }
    }
}
