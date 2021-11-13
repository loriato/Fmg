using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Motivo;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class MotivoRepository : NHibernateRepository<Motivo>
    {
        public IQueryable<Motivo> Listar()
        {
            var query = Queryable();
            return query;
        }

        public List<Motivo> Listar(FiltroMotivoDto filtro)
        {
            var query = Queryable();

            if (filtro.TipoMotivo.HasValue())
            {
                query = query.Where(x => x.TipoMotivo == filtro.TipoMotivo);
            }
            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => x.Situacao == filtro.Situacao);
            }

            return query.ToList();
        }
    }
}
