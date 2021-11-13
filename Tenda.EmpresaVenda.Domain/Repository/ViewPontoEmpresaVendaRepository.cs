using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPontoEmpresaVendaRepository : NHibernateRepository<ViewPontoEmpresaVenda>
    {
        public IQueryable<ViewPontoEmpresaVenda> Listar(PontoVendaDto filtro)
        {
            var query = Queryable();
            if (filtro.Nome.HasValue())
            {
                query = query.Where(x => x.NomePontoEmpresaVenda.ToLower().Contains(filtro.Nome.ToLower()));
            }
            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => filtro.Situacao.Contains((int)x.SituacaoPontoVenda));
            }
            if (filtro.IniciativaTenda.HasValue())
            {
                query = query.Where(x => x.IniciativaTenda == filtro.IniciativaTenda);
            }
            if (filtro.idEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.IdEmpresaVenda == filtro.idEmpresaVenda);
            }
            if (filtro.idGerente.HasValue())
            {
                query = query.Where(x => x.IdGerentePontoVenda == filtro.idGerente);
            }
            if (filtro.IdPontosVenda.HasValue())
            {
                query = query.Where(x => filtro.IdPontosVenda.Contains(x.Id));
            }
            return query;
        }
    }
}
