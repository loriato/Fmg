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
    public class ViewCargarelatorioRepasseJunixRepository : NHibernateRepository<ViewCargarelatorioRepasseJunix>
    {
        public DataSourceResponse<ViewCargarelatorioRepasseJunix> ListarDatatable(DataSourceRequest request, CargaRelatorioRepasseDTO filtro)
        {
            var query = Queryable();

            if (filtro.DataInicio.HasValue())
            {
                query = query.Where(x => x.DataInicio.Value.Date >= filtro.DataInicio.Value.Date);
            }

            if (filtro.DataFim.HasValue())
            {
                query = query.Where(x => x.DataFim.Value.Date <= filtro.DataFim.Value.Date);
            }
            if (filtro.DataCriacaoInicio.HasValue())
            {
                query = query.Where(x => x.CriadoEm.Date >= filtro.DataCriacaoInicio.Date);
            }
            if(filtro.DataCriacaoFim.HasValue())
            {
                query = query.Where(x => x.CriadoEm.Date <= filtro.DataCriacaoFim.Date);
            }
            if (filtro.NomeArquivo.HasValue())
            {
                query = query.Where(x => x.NomeArquivo.Contains(filtro.NomeArquivo));
            }
            if (filtro.CriadoPor.HasValue())
            {
                query = query.Where(x => x.NomeUsuario.Contains(filtro.CriadoPor));
            }
            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => x.Situacao == filtro.Situacao);
            }
            if (filtro.Origem.HasValue())
            {
                query = query.Where(x => x.Origem == filtro.Origem);
            }
            return query.ToDataRequest(request);
        }
    }
}
