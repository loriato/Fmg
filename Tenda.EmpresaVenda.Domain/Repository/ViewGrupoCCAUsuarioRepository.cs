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
    public class ViewGrupoCCAUsuarioRepository:NHibernateRepository<ViewGrupoCCAUsuario>
    {
        public DataSourceResponse<ViewGrupoCCAUsuario> ListarDatatable(DataSourceRequest request, GrupoCCADTO filtro)
        {
            var query = Queryable().Where(x => x.IdGrupoCCA == filtro.IdGrupoCCA);

            if (!filtro.NomeUsuario.IsEmpty())
            {
                filtro.NomeUsuario = filtro.NomeUsuario.ToLower();
                query = query.Where(x => x.NomeUsuario.ToLower().Contains(filtro.NomeUsuario));
            }

            return query.ToDataRequest(request);
        }
    }
}
