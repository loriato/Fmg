

using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class NotaFiscalPagamentoRepository : NHibernateRepository<NotaFiscalPagamento>
    {
        public List<NotaFiscalPagamento> BuscarNotasFicaisPagamentosPorId(List<long> idsNotaFiscalPagamento)
        {
            return Queryable().Where(x => idsNotaFiscalPagamento.Contains(x.Id))
                .Where(x => x.Arquivo != null)
                .ToList();
        }
        public List<long> FindIdsNotaFiscalPagamentoAguardandoProcessamento()
        {
            return Queryable().Where(x => x.Situacao == SituacaoNotaFiscal.AguardandoProcessamento).Select(x => x.Id).ToList();
        }
        public List<NotaFiscalPagamento> FindNotaFiscalPagamentoPreAprovado()
        {
            return Queryable().Where(x => x.Situacao == SituacaoNotaFiscal.PreAprovado).ToList();
        }
    }
}
