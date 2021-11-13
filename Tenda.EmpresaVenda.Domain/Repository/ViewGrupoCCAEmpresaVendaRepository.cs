using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewGrupoCCAEmpresaVendaRepository : NHibernateRepository<ViewGrupoCCAEmpresaVenda>
    {
        public DataSourceResponse<ViewGrupoCCAEmpresaVenda>ListarDatatable(DataSourceRequest request, GrupoCCAEmpresaDTO filtro)
        {
            var query = Queryable().Where(x=>x.IdGrupoCCA == filtro.IdGrupoCCA);

            if (filtro.IdRegional.HasValue())
            {
                query = query.Where(x => x.IdRegional.Contains('#' + filtro.IdRegional.ToString() + '#'));
            }

            if (!filtro.UF.IsEmpty() && !filtro.UF.Contains(""))
            {
                query = query.Where(x => filtro.UF.Contains(x.UF));
            }

            if (!filtro.NomeEmpresaVenda.IsEmpty())
            {
                filtro.NomeEmpresaVenda = filtro.NomeEmpresaVenda.ToLower();
                query = query.Where(x => x.NomeEmpresaVenda.ToLower().Contains(filtro.NomeEmpresaVenda));
            }

                
            return query.ToDataRequest(request);
        }

    }
}
