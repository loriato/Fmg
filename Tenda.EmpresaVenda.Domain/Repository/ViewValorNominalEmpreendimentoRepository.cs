using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewValorNominalEmpreendimentoRepository : NHibernateRepository<ViewValorNominalEmpreendimento>
    {
        public IQueryable<ViewValorNominalEmpreendimento> Listar(FiltroValorNominalEmpreendimentoDTO filtro)
        {
            var query = Queryable();
            if (filtro.IdEmpreendimento.HasValue())
            {
                query = query.Where(reg => reg.IdEmpreendimento == filtro.IdEmpreendimento);
            }
            if (filtro.Situacao.HasValue())
            {
                query = query.Where(reg => reg.Situacao == filtro.Situacao);
            }
            if (filtro.Estados.HasValue())
            {
                query = query.Where(reg => filtro.Estados.Contains(reg.Estado));
            }
            if (filtro.Estado.HasValue())
            {
                query = query.Where(reg => reg.Estado.ToUpper() == filtro.Estado.ToUpper());
            }
            if (filtro.Nome.HasValue())
            {
                query = query.Where(reg => reg.NomeEmpreendimento.ToUpper().Contains(filtro.Nome.ToUpper()));
            }
            if (filtro.VigenteEm.HasValue())
            {
                query = query.Where(reg => reg.InicioVigencia.Value.Date <= filtro.VigenteEm.Value.Date)
                    .Where(reg => reg.TerminoVigencia.Value.Date >= filtro.VigenteEm.Value.Date || !reg.TerminoVigencia.HasValue);
            }

            return query;
        }
    }
}
