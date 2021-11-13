using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.Loja;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewLojasPortalRepository : NHibernateRepository<ViewLojasPortal>
    {
        public RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }

        public DataSourceResponse<ViewLojasPortal> Listar(FiltroLojaPortalDto filtro)
        {
            var query = Queryable();

            if (filtro.Nome.HasValue())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(filtro.Nome.ToLower()));
            }
            if (!filtro.IdRegional.IsEmpty() && (!filtro.Estado.IsEmpty() && !filtro.Estado.Contains("Todos")))
            {
                var list = _regionalEmpresaRepository.Queryable()
                    .Where(w => (w.EmpresaVenda.ConsiderarUF == TipoSimNao.Sim &&
                    filtro.IdRegional.Contains(w.Regional.Id) &&
                    filtro.Estado.Contains(w.EmpresaVenda.Estado)) ||
                    (w.EmpresaVenda.ConsiderarUF == TipoSimNao.Nao &&
                    filtro.IdRegional.Contains(w.Regional.Id)))
                    .Select(s => s.EmpresaVenda.Id);

                query = query.Where(w => list.Contains(w.Id));
            }
            if ((!filtro.Estado.IsEmpty() && !filtro.Estado.Contains("Todos")) && filtro.IdRegional.IsEmpty())
            {
                query = query.Where(w => filtro.Estado.Contains(w.Estado));
            }
            if (!filtro.IdRegional.IsEmpty() && (filtro.Estado.IsEmpty() || filtro.Estado.Contains("Todos")))
            {
                var list = _regionalEmpresaRepository.Queryable().Where(w => filtro.IdRegional.Contains(w.Regional.Id)).Select(s => s.EmpresaVenda.Id);
                query = query.Where(w => list.Contains(w.Id));
            }


            var dataSource = query.ToDataRequest(filtro.DataSourceRequest);
            return dataSource;
        }
    }
}
