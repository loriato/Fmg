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
    public class ViewResgatePontuacaoFidelidadeRepository : NHibernateRepository<ViewResgatePontuacaoFidelidade>
    {
        public DataSourceResponse<ViewResgatePontuacaoFidelidade> ListarResgate(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var query = Queryable();

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.IdsEmpresaVenda.HasValue())
            {
                query = query.Where(x => filtro.IdsEmpresaVenda.Contains(x.IdEmpresaVenda));
            }

            if (filtro.PeriodoDe.HasValue() && !filtro.DataLiberacao && !filtro.DataSolicitacao)
            {
                query = query.Where(x => x.DataResgate.Date >= filtro.PeriodoDe.Value.Date);
            }

            if (filtro.PeriodoAte.HasValue() && !filtro.DataLiberacao && !filtro.DataSolicitacao)
            {
                query = query.Where(x => x.DataResgate.Date <= filtro.PeriodoAte.Value.Date);
            }

            if (filtro.SituacaoResgate.HasValue())
            {
                query = query.Where(x => x.SituacaoResgate == filtro.SituacaoResgate);
            }

            if (filtro.DataSolicitacao)
            {
                if (filtro.PeriodoDe.HasValue())
                {
                    query = query.Where(x => x.DataResgate.Date >= filtro.PeriodoDe.Value.Date);
                }

                if (filtro.PeriodoAte.HasValue())
                {
                    query = query.Where(x => x.DataResgate.Date <= filtro.PeriodoAte.Value.Date);
                }
            }

            if (filtro.DataLiberacao)
            {
                if (filtro.PeriodoDe.HasValue())
                {
                    query = query.Where(x => x.DataLiberacao.Value.Date >= filtro.PeriodoDe.Value.Date);
                }

                if (filtro.PeriodoAte.HasValue())
                {
                    query = query.Where(x => x.DataLiberacao.Value.Date <= filtro.PeriodoAte.Value.Date);
                }
            }

            return query.ToDataRequest(request);
        }
    }
}
