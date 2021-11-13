using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewRegraComissaoPadraoRepository : NHibernateRepository<ViewRegraComissaoPadrao>
    {

        public IQueryable<ViewRegraComissaoPadrao> Listar(FiltroRegraComissaoDTO filtro)
        {
            var result = Queryable();
            if (!filtro.Descricao.IsEmpty())
            {
                result = result.Where(reg => reg.Descricao.ToLower().Contains(filtro.Descricao.ToLower()));
            }
            if (!filtro.Situacoes.IsEmpty())
            {
                result = result.Where(reg => filtro.Situacoes.Contains(reg.Situacao));
            }
            if (!filtro.Regionais.IsEmpty())
            {
                result = result.Where(reg => filtro.Regionais.Contains(reg.Regional));
            }
            if (!filtro.VigenteEm.IsEmpty())
            {
                result = result.Where(reg => reg.InicioVigencia.Value.Date <= filtro.VigenteEm.Value.Date)
                    .Where(reg => reg.TerminoVigencia.Value.Date >= filtro.VigenteEm.Value.Date || !reg.TerminoVigencia.HasValue);
            }
            return result;
        }
    }
}
