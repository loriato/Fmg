using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewRelatorioComissaoRepository : NHibernateRepository<ViewRelatorioComissao>
    {
        public IQueryable<ViewRelatorioComissao> Listar(RelatorioComissaoDTO filtro)
        {
            var query = Queryable();

            if (filtro.TipoEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.TipoEmpresaVenda == filtro.TipoEmpresaVenda);
            }

            if (filtro.Faturado.HasValue())
            {
                var faturado = filtro.Faturado == 1;
                query = query.Where(x => x.Faturado == faturado);
            }

            if (filtro.DataFaturadoDe.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date >= filtro.DataFaturadoDe.Value.Date);
            }

            if (filtro.DataFaturadoAte.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date <= filtro.DataFaturadoAte.Value.Date);
            }

            if (filtro.IdEmpreendimento.HasValue())
            {
                query = query.Where(x => x.IdEmpreendimento == filtro.IdEmpreendimento);
            }
            if (!filtro.CodigoPreProposta.IsEmpty())
            {
                query = query.Where(x => x.CodigoPreProposta.ToUpper().Equals(filtro.CodigoPreProposta.ToUpper()));
            }
            if (!filtro.CodigoProposta.IsEmpty())
            {
                query = query.Where(x => x.CodigoProposta.ToUpper().Equals(filtro.CodigoProposta.ToUpper()));
            }
            if (filtro.Estados.HasValue())
            {
                query = query.Where(x => filtro.Estados.Contains(x.Estado.ToUpper()));
            }
            if (filtro.Regionais.HasValue())
            {
                query = query.Where(x => filtro.Regionais.Contains(x.IdRegional));
            }
            if (!filtro.NomeFornecedor.IsEmpty())
            {
                query = query.Where(x => x.NomeFornecedor.ToUpper().Equals(filtro.NomeFornecedor.ToUpper()));
            }
            if (!filtro.CodigoFornecedor.IsEmpty())
            {
                query = query.Where(x => x.CodigoFornecedor.ToUpper().Equals(filtro.CodigoFornecedor.ToUpper()));
            }
            if (filtro.DataVendaDe.HasValue())
            {
                query = query.Where(x => x.DataVenda.Value.Date >= filtro.DataVendaDe.Value.Date);
            }
            if (filtro.DataVendaAte.HasValue())
            {
                query = query.Where(x => x.DataVenda.Value.Date <= filtro.DataVendaAte.Value.Date);
            }
            if (!filtro.NomeCliente.IsEmpty())
            {
                query = query.Where(x => x.NomeCliente.ToLower().Contains(filtro.NomeCliente.ToLower()));
            }
            if (!filtro.StatusContrato.IsEmpty())
            {
                query = query.Where(x => x.StatusContrato.ToLower().Equals(filtro.StatusContrato.ToLower()));
            }
            if (filtro.IdsEmpresaVenda.HasValue())
            {
                query = query.Where(x => filtro.IdsEmpresaVenda.Contains(x.IdEmpresaVenda));
            }
            if (filtro.TipoPagamento.HasValue())
            {
                query = query.Where(x => x.TipoPagamento == filtro.TipoPagamento);
            }
            if (filtro.AdiantamentoPagamento.HasValue())
            {
                query = query.Where(x => x.AdiantamentoPagamento == filtro.AdiantamentoPagamento);
            }
            if (filtro.PontosVenda.HasValue())
            {
                query = query.Where(x => filtro.PontosVenda.Contains(x.IdPontoVenda));
            }
            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }
            return query;
        }

        public IQueryable<ViewRelatorioComissao> ListarAdiantamentoEmergencial(RelatorioComissaoDTO filtro)
        {
            
            var query = Listar(filtro);

            query = query.Where(x => x.KitCompleto)
                  .Where(x => (x.DataConformidade == null && x.TipoPagamento == TipoPagamento.Conformidade) ||
                  (x.DataRepasse == null && x.TipoPagamento == TipoPagamento.Repasse))
                  .Where(x => x.TipoPagamento != TipoPagamento.KitCompleto)
                  .Where(x => x.AdiantamentoPagamento != StatusAdiantamentoPagamento.Solicitado);
            return query;
        }

        public IQueryable<ViewRelatorioComissao> ListarAdiantamentoEmergencialSolicitado(RelatorioComissaoDTO filtro)
        {
            
            var query = Listar(filtro);

            query = query.Where(x=>x.Faturado)
                .Where(x => x.AdiantamentoPagamento == StatusAdiantamentoPagamento.Solicitado ||
                                 x.AdiantamentoPagamento == StatusAdiantamentoPagamento.Aprovado);
            return query;
        }
    }
}

