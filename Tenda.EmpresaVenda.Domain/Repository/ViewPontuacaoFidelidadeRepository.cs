using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPontuacaoFidelidadeRepository : NHibernateRepository<ViewPontuacaoFidelidade>
    {
        public DataSourceResponse<ViewPontuacaoFidelidade> ListarDatatablePontuacaoFidelidade(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var query = Queryable();

            if (filtro.Regional.HasValue())
            {
                query = query.Where(x => x.Regional.Equals(filtro.Regional));
            }

            if (filtro.Descricao.HasValue())
            {
                filtro.Descricao = filtro.Descricao.ToLower();
                query = query.Where(x => x.Descricao.ToLower().Contains(filtro.Descricao));
            }

            if (filtro.IdEmpreendimentos.HasValue())
            {
                query = query.Where(x => filtro.IdEmpreendimentos.Contains(x.IdEmpreendimento));
            }

            if (filtro.VigenteEm.HasValue())
            {
                query = query.Where(x => x.InicioVigencia.Value.Date <= filtro.VigenteEm.Value.Date)
                    .Where(x => x.TerminoVigencia.Value.Date >= filtro.VigenteEm.Value.Date || !x.TerminoVigencia.HasValue);
            }

            if (filtro.TipoPontuacaoFidelidade.HasValue())
            {
                query = query.Where(x => x.TipoPontuacaoFidelidade == filtro.TipoPontuacaoFidelidade);
            }

            if (filtro.TipoCampanhaFidelidade.HasValue())
            {
                query = query.Where(x => x.TipoCampanhaFidelidade == filtro.TipoCampanhaFidelidade);
            }

            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => x.Situacao == filtro.Situacao);
            }

            if (filtro.IdsEmpresaVenda.HasValue())
            {
                query = query.Where(x => filtro.IdsEmpresaVenda.Contains(x.IdEmpresaVenda));
            }

            if (filtro.Codigo.HasValue())
            {
                filtro.Codigo = filtro.Codigo.ToUpper();
                query = query.Where(x => x.Codigo.Contains(filtro.Codigo));
            }

            return query.ToDataRequest(request);
        }
    }
}
