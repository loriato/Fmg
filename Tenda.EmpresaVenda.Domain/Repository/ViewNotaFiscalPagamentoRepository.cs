using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewNotaFiscalPagamentoRepository : NHibernateRepository<ViewNotaFiscalPagamento>
    {
        public OcorrenciasMidasRepository _ocorrenciasMidasRepository { get; set; }

        public IQueryable<ViewNotaFiscalPagamento> Listar(FiltroPagamentoDTO filtro)
        {
            var query = MontarQuery(filtro);
            return query;
        }

        public IQueryable<FinanceiroGrupoDTO> ListarComGrupo(FiltroPagamentoDTO filtro)
        {
            var query = MontarQuery(filtro);

            var list = query.ToList();

            //Agrupa por pedidoSap e busca apenas os primeiros
            var distinct = list.GroupBy(x => new { x.PedidoSap, x.IdEmpresaVenda }).Select(y => y.First());

            var lista = new List<FinanceiroGrupoDTO>();

            foreach (var grupo in distinct)
            {
                var financeiro = new FinanceiroGrupoDTO
                {
                    IdEmpresaVenda = grupo.IdEmpresaVenda,
                    PedidoSap = grupo.PedidoSap,
                    IdNotaFiscalPagamento = grupo.IdNotaFiscalPagamento,
                    Filhos = list.Where(x => x.PedidoSap.Equals(grupo.PedidoSap))
                                .Where(x => x.IdEmpresaVenda.Equals(grupo.IdEmpresaVenda)).ToList()
                };
                lista.Add(financeiro);

            };

            return lista.AsQueryable();
        }


        public IQueryable<ViewNotaFiscalPagamento> MontarQuery(FiltroPagamentoDTO filtro)
        {
            var query = Queryable();

            var pedidosSap = BuscarPedidosSap(filtro);
            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.IdsEmpresaVenda.HasValue())
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
                query = query.Where(x => x.NotaFiscal.ToUpper().Contains(filtro.NumeroNotaFiscal.ToUpper()));
            }

            if (filtro.Faturado.HasValue())
            {
                var faturado = filtro.Faturado == 1;
                query = query.Where(x => x.Faturado == faturado);
            }
            // As situações Distratado e Pendente de Envio não existem no banco e portanto precisam ser condicionanadas no filtro
            if (!filtro.Situacoes.IsEmpty())
            {

                if (filtro.Situacoes.Count() > 1)
                {
                    if (filtro.Situacoes.Contains(SituacaoNotaFiscal.PendenteEnvio) && filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
                    {
                        query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) || (reg.IdNotaFiscalPagamento == null && !reg.Pago) || reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper());
                    }
                    else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.PendenteEnvio))
                    {
                        query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) || (reg.IdNotaFiscalPagamento == null && !reg.Pago) && !(reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper()));
                    }
                    else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
                    {
                        query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) || reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper());
                    }
                    else if (!filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
                    {   
                        query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) && !(reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper()));
                    }

                }
                else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.PendenteEnvio))
                {
                    query = query.Where(reg => (reg.IdNotaFiscalPagamento == null && !reg.Pago) && (reg.PassoAtual.ToUpper() != "Prop. Cancelada".ToUpper() && !reg.EmReversao));
                }
                else if (filtro.Situacoes.Contains(SituacaoNotaFiscal.Distratado))
                {
                    query = query.Where(reg => reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper());
                }
                else
                {
                    query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoNotaFiscal) && (reg.PassoAtual.ToUpper() != "Prop. Cancelada".ToUpper() && !reg.EmReversao));
                }
            }
            if (!filtro.Situacao.IsEmpty())
            {
                if (filtro.Situacao == SituacaoNotaFiscal.PendenteEnvio)
                {
                    query = query.Where(reg => reg.IdNotaFiscalPagamento == null && !reg.Pago && reg.PassoAtual.ToUpper() != "Prop. Cancelada".ToUpper() && !reg.EmReversao);
                }
                else if (filtro.Situacao == SituacaoNotaFiscal.Distratado)
                {
                    query = query.Where(reg => reg.EmReversao == true || reg.PassoAtual.ToUpper() == "Prop. Cancelada".ToUpper());
                }
                else
                {
                    query = query.Where(reg => reg.SituacaoNotaFiscal == filtro.Situacao);
                }
            }
            if(!filtro.Regionais.IsEmpty())
            {
                query = query.Where(w => filtro.Regionais.Contains(w.IdRegional));
            }
            return query;
        }

        private List<string> BuscarPedidosSap(FiltroPagamentoDTO filtro)
        {

            var query = Queryable();

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.DataInicio.HasValue())
            {
                query = query.Where(reg => reg.DataComissao.Value.Date >= filtro.DataInicio.Value.Date);
            }

            if (filtro.DataTermino.HasValue())
            {
                query = query.Where(reg => reg.DataComissao.Value.Date <= filtro.DataTermino.Value.Date);
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

        public List<ViewNotaFiscalPagamento> BuscarPagamentos(long idEmpresaVenda, string pedidoSap)
        {
            return Queryable().Where(reg => reg.IdEmpresaVenda == idEmpresaVenda)
                .Where(reg => reg.PedidoSap.ToLower() == pedidoSap.ToLower()).ToList();
        }
        public List<ViewNotaFiscalPagamento> BuscarNotasFicaisPorIds()
        {
            var result = Queryable().Where(x => x.SituacaoNotaFiscal == SituacaoNotaFiscal.PreAprovado)
                .GroupBy(x => new { x.IdNotaFiscalPagamento, x.PedidoSap })
                .Select(x => new ViewNotaFiscalPagamento
                {
                    IdNotaFiscalPagamento = x.Key.IdNotaFiscalPagamento,
                    PedidoSap = x.Key.PedidoSap
                })
                .ToList();

            return result;
        }
        public List<ViewNotaFiscalPagamento> FindIdsNotaFiscalPagamentoAguardandoProcessamento()
        {
            var result = Queryable().Where(x => x.SituacaoNotaFiscal == SituacaoNotaFiscal.AguardandoProcessamento )
                            .GroupBy(x => new { x.IdNotaFiscalPagamento, x.CnpjEmpresaVenda, x.NotaFiscal, x.IdEmpreendimento, x.Estado })
                            .Select(x => new ViewNotaFiscalPagamento
                            {
                                IdNotaFiscalPagamento = x.Key.IdNotaFiscalPagamento,
                                CnpjEmpresaVenda = x.Key.CnpjEmpresaVenda,
                                NotaFiscal = x.Key.NotaFiscal,
                                IdEmpreendimento = x.Key.IdEmpreendimento,
                                Estado = x.Key.Estado
                            })
                            .ToList();

            return result;
        }
        public long BuscaIdNotaFiscalPorIdPagamento(string ocorrenceId)
        {
            return Queryable().Where(x => x.IdPagamento == Int64.Parse(ocorrenceId)).Select(x => x.IdNotaFiscalPagamento).FirstOrDefault().Value;

        }
        public ViewNotaFiscalPagamento FindByIdPagamento(long idPagamento)
        {
            return Queryable().Where(x => x.IdPagamento == idPagamento).FirstOrDefault();
        }

    }
}
