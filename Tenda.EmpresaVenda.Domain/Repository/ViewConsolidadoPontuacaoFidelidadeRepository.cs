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
    public class ViewConsolidadoPontuacaoFidelidadeRepository : NHibernateRepository<ViewConsolidadoPontuacaoFidelidade>
    {
        public DataSourceResponse<ViewConsolidadoPontuacaoFidelidade> ListarPontuacao(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var query = Queryable();

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.InicioVigencia.HasValue())
            {
                query = query.Where(x => x.DataPontuacao.Date >= filtro.InicioVigencia.Value.Date);
            }

            if (filtro.TerminoVigencia.HasValue())
            {
                query = query.Where(x => x.DataPontuacao.Date <= filtro.TerminoVigencia.Value.Date);
            }

            return query.ToDataRequest(request);
        }
    }
}
