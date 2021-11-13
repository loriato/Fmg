using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class FaseStatusContratoJunixRepository : NHibernateRepository<FaseStatusContratoJunix>
    {
        public DataSourceResponse<FaseStatusContratoJunix> ListarFases(DataSourceRequest request, JunixDTO filtro)
        {
            var query = Queryable();

            if (!filtro.Fase.IsEmpty())
            {
                filtro.Fase = filtro.Fase.ToLower();
                query = query.Where(x => x.Fase.ToLower().Contains(filtro.Fase)) ;
            }

            return query.ToDataRequest(request);

        }

        public Boolean CheckIfExistsFaseStatusContratoJunix(FaseStatusContratoJunix obj)
        {
            var query = Queryable();
            if (!obj.Id.IsEmpty())
            {
                query = query.Where(x => x.Id != obj.Id);
            }

            var fase = obj.Fase.ToLower();
            query = query.Where(x => x.Fase.ToLower().Equals(fase));
            return query.Any();
        }
    }
}
