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
    public class StatusSuatStatusEvsRepository : NHibernateRepository<StatusSuatStatusEvs>
    {
        public IQueryable<StatusSuatStatusEvs> Listar()
        {
            return Queryable();
        }
        public IQueryable<StatusSuatStatusEvs> Listar(StatusSuatStatusEvsDTO filtro)
        {
            var query = Queryable();
            if (filtro.statusSuatEvs.HasValue())
            {
                query = query.Where(x => x.DescricaoSuat.ToLower().Contains(filtro.statusSuatEvs.ToLower())
                || x.DescricaoEvs.ToLower().Contains(filtro.statusSuatEvs.ToLower()));
            }

            return query;
        }

        public bool CheckIfExistsStatusSuat(StatusSuatStatusEvs model)
        {
            return Queryable().Where(pntv => pntv.Id != model.Id)
                              .Where(pntv => pntv.DescricaoSuat.ToLower().Equals(model.DescricaoSuat.ToLower()))
                              .Any();
        }
    }
}
