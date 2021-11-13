using Europa.Data;
using Europa.Extensions;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewRelatorioVendaUnificadoRepository:NHibernateRepository<ViewRelatorioVendaUnificado>
    {
        public DataSourceResponse<ViewRelatorioVendaUnificado>ListarRelatorioVendaUnificado(DataSourceRequest request, RelatorioComissaoDTO filtro)
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

            if (filtro.PeriodoDe.HasValue())
            {
                query = query.Where(x => filtro.PeriodoDe.Date <= x.DataComissao.Value.Date);
            }

            if (filtro.PeriodoAte.HasValue())
            {
                query = query.Where(x => filtro.PeriodoAte.Date >= x.DataComissao.Value.Date);
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

            if (filtro.DataPrevisaoPagamentoInicio.HasValue())
            {
                query = query.Where(x => x.DataPrevisaoPagamento.Value.Date >= filtro.DataPrevisaoPagamentoInicio.Value.Date);
            }

            if (filtro.DataPrevisaoPagamentoTermino.HasValue())
            {
                query = query.Where(x => x.DataPrevisaoPagamento.Value.Date <= filtro.DataPrevisaoPagamentoTermino.Value.Date);
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
            if (filtro.Estados.HasValue()&& !filtro.Estados.Contains(""))
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
                query = query.Where(x => x.CodigoEmpresa.ToUpper().Equals(filtro.CodigoFornecedor.ToUpper()));
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

            if (filtro.StatusIntegracaoSap.HasValue())
            {
                if (filtro.StatusIntegracaoSap == StatusIntegracaoSap.AguardandoRC)
                {
                    query = query.Where(x => x.StatusIntegracaoSap == filtro.StatusIntegracaoSap||x.StatusIntegracaoSap==null);
                }
                else
                {
                    query = query.Where(x => x.StatusIntegracaoSap == filtro.StatusIntegracaoSap);
                }
            }

            if (filtro.Pago.HasValue())
            {
                bool pago = filtro.Pago == 1;
                query = query.Where(reg => reg.Pago == pago);
            }

            if (filtro.DataRcPedidoSapDe.HasValue())
            {
                query = query.Where(x => (x.DataRequisicaoCompra != null && x.DataRequisicaoCompra.Value.Date >= filtro.DataRcPedidoSapDe.Value.Date) ||
                (x.DataPedidoSap != null && x.DataPedidoSap.Value.Date >= filtro.DataRcPedidoSapDe.Value.Date));
            }

            if (filtro.DataRcPedidoSapAte.HasValue())
            {
                query = query.Where(x => (x.DataRequisicaoCompra != null && x.DataRequisicaoCompra.Value.Date <= filtro.DataRcPedidoSapAte.Value.Date) ||
                (x.DataPedidoSap != null && x.DataPedidoSap.Value.Date <= filtro.DataRcPedidoSapAte.Value.Date));
            }

            return query.ToDataRequest(request);
        }

        public ViewRelatorioVendaUnificado RelacionarEmpresaVendaProposta(long idEmpresaVenda, string cdProposta)
        {
            var query = Queryable();
            query = query.Where(x => x.IdEmpresaVenda == idEmpresaVenda);
            query = query.Where(x => x.CodigoProposta.ToUpper().Contains(cdProposta.ToUpper()));
            return query.FirstOrDefault();
        }

    }
}
