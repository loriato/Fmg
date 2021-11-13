using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;


namespace Tenda.EmpresaVenda.Domain.Repository
{

    public class ConsultaAceitesContratosCorretagemRepository : NHibernateRepository<ViewConsultaAceitesContratosCorretagem>
    {
        public DataSourceResponse<ViewConsultaAceitesContratosCorretagem> Consultar(DataSourceRequest request, FiltroConsultaAceitesContratosCorretagemDTO filtro)
        {
            var query = Queryable();
            if (!filtro.IdEmpresasVenda.IsNull() && !filtro.IdEmpresasVenda.IsEmpty())
            {
                if (filtro.IdEmpresasVenda.Length == 1)
                {
                    query = query.Where(x => x.IdEmpresaVenda == filtro.IdEmpresasVenda[0]);
                }
                else
                {
                    List<int> filtroList = new List<int>(filtro.IdEmpresasVenda);
                    query = query.Where(x => filtroList.Contains(x.IdEmpresaVenda));

                }
            }
            return query.ToDataRequest(request);
        }

    }
}
