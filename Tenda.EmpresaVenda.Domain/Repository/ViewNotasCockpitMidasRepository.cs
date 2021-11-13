using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.CockpitMidas;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewNotasCockpitMidasRepository : NHibernateRepository<ViewNotasCockpitMidas>
    {

        public IQueryable<ViewNotasCockpitMidas> ListarTodasNotas()
        {
            return Queryable();
        }
        public DataSourceResponse<ViewNotasCockpitMidas> ListarNotas(FiltroCockpitMidas filtro)
        {
                var query = MontarQuery(filtro);            

            return query.ToDataRequest(filtro.Request);
        }

        public IQueryable<ViewNotasCockpitMidas> MontarQuery(FiltroCockpitMidas filtro)
        {
            var query = Queryable();

            var pedidosSap = BuscarPedidosSap(filtro);

            if (ProjectProperties.DataInicioMidas.HasValue())
            {
                query = query.Where(reg => reg.AtualizadoEm.Date >= ProjectProperties.DataInicioMidas.Date);
            }
            if (filtro.Match.HasValue())
            {
                query = query.Where(reg => Convert.ToInt32(reg.Match) == Convert.ToInt32((bool)filtro.Match));
            }

            if (filtro.Ocorrencia.HasValue())
            {
                query = query.Where(reg => reg.IdOcorrencia == filtro.Ocorrencia);
            }

            if (filtro.PreProposta.HasValue())
            {
                query = query.Where(reg => reg.CodigoPreProposta.Contains(filtro.PreProposta));
            }

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.IdsEmpresaVenda.HasValue() && filtro.IdsEmpresaVenda[0] != 0)
            {
                query = query.Where(reg => filtro.IdsEmpresaVenda.Contains(reg.IdEmpresaVenda));
            }

            if (filtro.Estado.HasValue() && !filtro.Estado.Contains(""))
            {
                query = query.Where(reg => filtro.Estado.Contains(reg.Estado.ToUpper()));
            }
            
           query = query.Where(reg => pedidosSap.Contains(reg.PedidoSap));

            if (filtro.NumeroPedido.HasValue())
            {
                query = query.Where(x => x.PedidoSap.ToUpper() == filtro.NumeroPedido.ToUpper());
            }

            if (filtro.NumeroNotaFiscal.HasValue())
            {
                query = query.Where(x => x.NotaFiscal.ToUpper().Contains(filtro.NumeroNotaFiscal.ToUpper()) || x.NFeMidas.ToUpper().Contains(filtro.NumeroNotaFiscal.ToUpper()));
            }

            if (!filtro.Situacoes.IsEmpty())
            {
                query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal));
            }

            if (filtro.CNPJPrestador.HasValue())
            {
                query = query.Where(reg => reg.CNPJEmpresaVenda.Contains(filtro.CNPJPrestador.OnlyNumber()));
            }
            if (filtro.CNPJTomador.HasValue())
            {
                query = query.Where(reg => reg.CNPJTomador.Contains(filtro.CNPJTomador.OnlyNumber()));
            }
            //if (!filtro.Situacoes.IsEmpty())
            //{

            //    if (filtro.Situacoes.Count() > 1)
            //    {
            //        if (filtro.Situacoes.Contains(SituacaoNotaFiscal.PendenteEnvio) && filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
            //        {
            //            query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) || (reg.IdNotaFiscalPagamento == null && !reg.Pago) || reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper());
            //        }
            //        else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.PendenteEnvio))
            //        {
            //            query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) || (reg.IdNotaFiscalPagamento == null && !reg.Pago) && !(reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper()));
            //        }
            //        else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
            //        {
            //            query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) || reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper());
            //        }
            //        else if (!filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
            //        {
            //            query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) && !(reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper()));
            //        }

            //    }
            //    else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.PendenteEnvio))
            //    {
            //        query = query.Where(reg => (reg.IdNotaFiscalPagamento == null && !reg.Pago) && (reg.PassoAtual.ToUpper() != "Prop. Cancelada".ToUpper() && !reg.EmReversao));
            //    }
            //    else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
            //    {
            //        query = query.Where(reg => reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper());
            //    }
            //    else
            //    {
            //        query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) && (reg.PassoAtual.ToUpper() != "Prop. Cancelada".ToUpper() && reg.EmReversao == false));
            //    }
            //}

            return query;
        }

        private List<string> BuscarPedidosSap(FiltroCockpitMidas filtro)
        {

            var query = Queryable();

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.DataPrevisaoPagamentoInicio.HasValue())
            {
                query = query.Where(reg => reg.DataPrevisaoPagamento.Value.Date >= filtro.DataPrevisaoPagamentoInicio.Value.Date);
            }

            if (filtro.DataPrevisaoPagamentoTermino.HasValue())
            {
                query = query.Where(reg => reg.DataPrevisaoPagamento.Value.Date <= filtro.DataPrevisaoPagamentoTermino.Value.Date);
            }


            return query.Select(x => x.PedidoSap).Distinct().ToList();
        }

    }
}
