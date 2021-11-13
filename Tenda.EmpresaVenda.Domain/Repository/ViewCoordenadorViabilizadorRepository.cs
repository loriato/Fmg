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
    public class ViewCoordenadorViabilizadorRepository:NHibernateRepository<ViewCoordenadorViabilizador>
    {
        public DataSourceResponse<ViewCoordenadorViabilizador>ListarViabilizadores(DataSourceRequest request, HierarquiaCicloFinanceiroDTO filtro)
        {
            var query = Queryable().Where(x=>x.IdCoordenador==filtro.IdCoordenador);

            if (filtro.NomeViabilizador.HasValue())
            {
                query = query.Where(x => x.NomeViabilizador.ToUpper().Contains(filtro.NomeViabilizador.ToUpper()));
            }

            return query.ToDataRequest(request);
        }
        public DataSourceResponse<ViewCoordenadorViabilizador> ListarTodosViabilizadores(DataSourceRequest request)
        {
            return  Queryable().ToDataRequest(request);
        }

    }
}
