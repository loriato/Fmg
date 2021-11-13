using Europa.Data;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPagamentoRepository : NHibernateRepository<ViewPagamento>
    {
        private DateTime DataVendaGerada = ProjectProperties.DataBuscaVendaGerada.HasValue() ? ProjectProperties.DataBuscaVendaGerada : new DateTime(2020, 6, 1);

        public DataSourceResponse<ViewPagamento> ListarPagamentos(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var query = Queryable();

            if (filtro.DataVendaDe.IsEmpty())
            {
                filtro.DataVendaDe = DataVendaGerada;
            }

            if (filtro.IdsEmpresaVenda.HasValue())
            {
                query = query.Where(x => filtro.IdsEmpresaVenda.Contains(x.IdEmpresaVenda));
            }

            if (!filtro.IdEmpresaVenda.IsEmpty())
            {
                query = query.Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.PeriodoDe.HasValue())
            {
                query = query.Where(x => filtro.PeriodoDe.Date <= x.DataComissao.Value.Date);
            }

            if (filtro.PeriodoAte.HasValue())
            {
                query = query.Where(x => filtro.PeriodoAte.Date >= x.DataComissao.Value.Date);
            }
            if (filtro.DataPrevisaoPagamentoInicio.HasValue())
            {
                query = query.Where(x => filtro.DataPrevisaoPagamentoInicio.Value <= x.DataPrevisaoPagamento.Value.Date);
            }

            if (filtro.DataPrevisaoPagamentoTermino.HasValue())
            {
                query = query.Where(x => filtro.DataPrevisaoPagamentoTermino.Value >= x.DataPrevisaoPagamento.Value.Date);
            }

            if (filtro.DataVendaDe.HasValue())
            {
                if (filtro.DataVendaDe.Value.Date < DataVendaGerada.Date)
                {
                    filtro.DataVendaDe = DataVendaGerada;
                }
                query = query.Where(x => filtro.DataVendaDe.Value.Date <= x.DataVenda.Value.Date);
            }

            if (filtro.DataVendaAte.HasValue())
            {
                query = query.Where(x => filtro.DataVendaAte.Value.Date >= x.DataVenda.Value.Date);
            }
            if (filtro.Estados.HasValue() && !filtro.Estados.Contains(""))
            {
                query = query.Where(x => filtro.Estados.Contains(x.Estado.ToUpper()));
            }

            if (filtro.Pago.HasValue())
            {
                bool filterValue = filtro.Pago == 1;
                if (filterValue)
                {
                    query = query.Where(reg => reg.Pago == filterValue);
                }

                else
                {
                    query = query.Where(reg => reg.Pago != true || reg.IdPagamento == null);
                }
            }
            if (filtro.NomeCliente.HasValue())
            {
                query = query.Where(x => x.NomeCliente.ToLower().Contains(filtro.NomeCliente.ToLower()));
            }
            if (filtro.TipoPagamento.HasValue())
            {
                query = query.Where(x => x.TipoPagamento == filtro.TipoPagamento);
            }
            if (filtro.CodigoProposta.HasValue())
            {
                query = query.Where(x => x.CodigoProposta.ToUpper().Contains(filtro.CodigoProposta.ToUpper()));

            }
                        
            if (filtro.StatusIntegracaoSap.HasValue())
            {
                switch (filtro.StatusIntegracaoSap)
                {
                    case StatusIntegracaoSap.AguardandoRC:
                        query = query.Where(x => x.StatusIntegracaoSap.Value == filtro.StatusIntegracaoSap.Value || x.StatusIntegracaoSap == null);
                        break;
                    default:
                        query = query.Where(x => x.StatusIntegracaoSap.Value == filtro.StatusIntegracaoSap.Value);
                        break;
                }
            }

            if (filtro.DataRcPedidoSapDe.HasValue())
            {
                query = query.Where(x => (x.DataRequisicaoCompra!=null&& x.DataRequisicaoCompra.Value.Date >= filtro.DataRcPedidoSapDe.Value.Date)||
                (x.DataPedidoSap!=null&& x.DataPedidoSap.Value.Date >= filtro.DataRcPedidoSapDe.Value.Date));
            }

            if (filtro.DataRcPedidoSapAte.HasValue())
            {
                query = query.Where(x => (x.DataRequisicaoCompra != null && x.DataRequisicaoCompra.Value.Date <= filtro.DataRcPedidoSapAte.Value.Date) ||
                (x.DataPedidoSap != null && x.DataPedidoSap.Value.Date <= filtro.DataRcPedidoSapAte.Value.Date));
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
            if(filtro.Regionais.HasValue())
            {
                query = query.Where(x => filtro.Regionais.Contains(x.IdRegional));
            }
            return query.ToDataRequest(request);
        }
        public ViewPagamento RelacionarEmpresaVendaProposta(long idEmpresaVenda, string cdProposta)
        {
            var query = Queryable();
            query = query.Where(x => x.IdEmpresaVenda == idEmpresaVenda);
            query = query.Where(x => x.CodigoProposta.ToUpper().Contains(cdProposta.ToUpper()));
            return query.FirstOrDefault();
        }
    }
}
