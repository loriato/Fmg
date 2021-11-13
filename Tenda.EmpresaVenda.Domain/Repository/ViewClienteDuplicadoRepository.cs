using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewClienteDuplicadoRepository:NHibernateRepository<ViewClienteDuplicado>
    {
        public DataSourceResponse<ViewClienteDuplicado>ListarClientes(DataSourceRequest request,HierarquiaCicloFinanceiroDTO filtro)
        {
            var query = Queryable().Where(x => x.IdCoordenadorSupervisor == filtro.IdCoordenador||x.IdCoordenadorViabilizador == filtro.IdCoordenador);

            if (filtro.NomeCliente.HasValue())
            {
                query = query.Where(x => x.NomeCliente.ToUpper().Contains(filtro.NomeCliente.ToUpper()));
            }

            var aux = query.GroupBy(c => c.CPF, c => c.Id,
                (key, c) => new
                {
                    Key = key,
                    Count = c.Count(),
                    Max = c.Max()
                }).Select(x => x.Max).ToList();

            query = query.Where(x => aux.Contains(x.Id));

            return query.ToDataRequest(request);
        }
    }
}
