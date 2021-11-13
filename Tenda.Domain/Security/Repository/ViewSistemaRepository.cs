using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewSistemaRepository : NHibernateStatelessRepository<ViewSistema>
    {
        public DataSourceResponse<ViewSistema> Listar(DataSourceRequest request, string codigo, string nome, string enderecoAcesso)
        {
            var consulta = Queryable();

            if (!codigo.IsEmpty())
            {
                consulta = consulta.Where(x => x.Codigo.ToLower().StartsWith(codigo.ToLower()));
            }
            if (!nome.IsEmpty())
            {
                consulta = consulta.Where(x => x.Nome.ToLower().StartsWith(nome.ToLower()));
            }
            if (!enderecoAcesso.IsEmpty())
            {
                consulta = consulta.Where(x => x.EnderecoAcesso.ToLower().StartsWith(enderecoAcesso.ToLower()));
            }

            return consulta.ToDataRequest(request);
        }
    }
}
