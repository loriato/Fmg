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
    public class ViewPontoVendaRepository : NHibernateRepository<ViewPontoVenda>
    {
        public DataSourceResponse<ViewPontoVenda> Listar(DataSourceRequest request, PontoVendaDto filtro)
        {
            var query = Queryable();

            if (filtro.Nome.HasValue())
            {
                query = query.Where(x => x.NomePontoVenda.ToLower().Contains(filtro.Nome.ToLower()));
            }
            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => filtro.Situacao.Contains((int)x.Situacao));
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
                query = query.Where(x => x.IdGerente == filtro.idGerente);
            }
            if (filtro.IdPontosVenda.HasValue())
            {
                query = query.Where(x => filtro.IdPontosVenda.Contains(x.Id));
            }
            if (filtro.IdViabilizador.HasValue())
            {
                query = query.Where(x => x.IdViabilizador == filtro.IdViabilizador);
            }

            return query.ToDataRequest(request);
        }
    }
}
