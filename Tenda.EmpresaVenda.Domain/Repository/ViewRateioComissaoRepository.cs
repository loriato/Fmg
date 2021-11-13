using Europa.Data;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewRateioComissaoRepository : NHibernateRepository<ViewRateioComissao>
    {
        public IQueryable<ViewRateioComissao> Listar(FiltroRateioComissaoDTO filtro)
        {
            var query = Queryable();
            if (filtro.IdContratada.HasValue())
            {
                query = query.Where(x => x.IdContratada == filtro.IdContratada);
            }
            if (filtro.IdContratante.HasValue())
            {
                query = query.Where(x => x.IdContratante == filtro.IdContratante);
            }
            if (filtro.IdEmpreendimento.HasValue())
            {
                query = query.Where(x => x.IdEmpreendimento == filtro.IdEmpreendimento);
            }
            if (!filtro.VigenteEm.IsEmpty())
            {
                query = query.Where(reg => reg.InicioVigencia.Value.Date <= filtro.VigenteEm.Value.Date)
                    .Where(reg => reg.TerminoVigencia.Value.Date >= filtro.VigenteEm.Value.Date || !reg.TerminoVigencia.HasValue);
            }

            return query;
        }

        public IQueryable<ViewRateioComissao> ListarPorEV(long idEmpresaVenda)
        {
            return Queryable().Where(reg => reg.IdContratada == idEmpresaVenda || reg.IdContratante == idEmpresaVenda)
                                .Where(reg => !reg.TerminoVigencia.HasValue || reg.TerminoVigencia.Value.Date <= DateTime.Now.Date)
                                .Where(reg => reg.Situacao == SituacaoRateioComissao.Ativo);

        }

        public IQueryable<ViewRateioComissao> ListarPorEVContratada(long idEmpresaVenda)
        {
            return Queryable().Where(reg => reg.IdContratada == idEmpresaVenda)
                                .Where(reg => !reg.TerminoVigencia.HasValue || reg.TerminoVigencia.Value.Date <= DateTime.Now.Date)
                                .Where(reg => reg.Situacao == SituacaoRateioComissao.Ativo);

        }

        public IQueryable<ViewRateioComissao> ListarPorEVContratante(long idEmpresaVenda)
        {
            return Queryable().Where(reg => reg.IdContratante == idEmpresaVenda)
                                .Where(reg => !reg.TerminoVigencia.HasValue || reg.TerminoVigencia.Value.Date <= DateTime.Now.Date)
                                .Where(reg => reg.Situacao == SituacaoRateioComissao.Ativo);

        }
    }
}
