using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewCoordenadorSupervisorRepository:NHibernateRepository<ViewCoordenadorSupervisor>
    {
        public DataSourceResponse<ViewCoordenadorSupervisor> ListarSupervisores(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var query = Queryable().Where(x => x.IdCoordenador == filtro.IdCoordenador);

            if (filtro.NomeSupervisor.HasValue())
            {
                query = query.Where(x => x.NomeSupervisor.ToUpper().Contains(filtro.NomeSupervisor.ToUpper()));
            }

            return query.ToDataRequest(request);
        }
        public DataSourceResponse<ViewCoordenadorSupervisor> ListarTodosSupervisores(DataSourceRequest request)
        {
            return Queryable().ToDataRequest(request);
        }
    }
}
