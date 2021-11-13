using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PagamentoRepository : NHibernateRepository<Pagamento>
    {
        public void RemoverPagamento(Pagamento entity)
        {
            if (entity.Pago)
            {
                entity.PedidoSap = null;
                _session.SaveOrUpdate(entity);
            }
            else
            {
                _session.Delete(entity);
            }
        }

        public long BuscarIdPagamentoAtivo(long idProposta, TipoPagamento tipoPagamento, long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.Proposta.Id == idProposta)
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.TipoPagamento == tipoPagamento)
                .Where(x => x.Situacao == Situacao.Ativo)
                .Select(x => x.Id)
                .FirstOrDefault();
        }
        public List<Pagamento> BuscarPagamentoPorPedidoSap(long idEmpresaVenda, string pedido)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                               .Where(x => x.PedidoSap.ToLower() == pedido.ToLower())
                               .Where(x => x.Situacao == Situacao.Ativo).ToList();
        }

        public List<NotaFiscalPagamento> BuscarNotasFiscaisPagamento(long idEmpresaVenda, string pedido)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                               .Where(x => x.PedidoSap.ToLower() == pedido.ToLower())
                               .Where(x => x.Situacao == Situacao.Ativo)
                               .Select(x => x.NotaFiscalPagamento).ToList();
        }

        public bool CheckExistNotaFiscal(long idEmpresaVenda, string notaFiscal, string pedido)
        {
            return Queryable().Where(x => x.PedidoSap.ToLower() != pedido.ToLower())
                                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                                .Where(x => x.NotaFiscalPagamento.NotaFiscal == notaFiscal).Any();
        }

        public DateTime BuscarDataCriacaoPagamento(long idEmpresaVenda, string pedido)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                               .Where(x => x.PedidoSap.ToLower() == pedido.ToLower())
                               .OrderBy(x => x.CriadoEm)
                               .Select(x => x.CriadoEm)
                               .FirstOrDefault();
        }
        public List<string> BuscarRCsSemNumeroPedido()
        {
            return Queryable().Where(x => x.Situacao == Situacao.Ativo)
                                .Where(x => x.ReciboCompra != null)
                                .Where(x => x.PedidoSap == null)
                                .Select(x => x.ReciboCompra)
                                .ToList();

        }
        public Pagamento BuscarPorRC(string requisicaoCompra)
        {
            return Queryable().Where(x => x.Situacao == Situacao.Ativo)
                                .Where(x => x.ReciboCompra == requisicaoCompra).FirstOrDefault();
        }

        public List<Pagamento> BuscarPorIdNotaFiscalPagamento(long idNotaFiscalPagamento)
        {
            return Queryable().Where(x => x.NotaFiscalPagamento.Id == idNotaFiscalPagamento)
                                .Where(x => x.Situacao == Situacao.Ativo)
                                .ToList();
        }
    }
}
