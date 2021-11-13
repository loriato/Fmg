using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class NotaFiscalPagamentoOcorrenciaRepository : NHibernateRepository<NotaFiscalPagamentoOcorrencia>
    {
        public NotaFiscalPagamentoOcorrencia FindComissionedByOccurrenceId(long idOcorrencia)
        {
            return Queryable().Where(x => x.Ocorrencia.OccurenceId == idOcorrencia)
                                .Where(x => x.NotaFiscalPagamento.Situacao == SituacaoNotaFiscal.PreAprovado)
                                 .FirstOrDefault();
        }
        public NotaFiscalPagamentoOcorrencia FindByOccurrenceId(long idOcorrencia)
        {
            return Queryable().Where(x => x.Ocorrencia.OccurenceId == idOcorrencia).FirstOrDefault();
        }
        public NotaFiscalPagamentoOcorrencia FindByNotaFiscalId(long idNotaFiscal)
        {
            return Queryable().Where(x => x.NotaFiscalPagamento.Id == idNotaFiscal).FirstOrDefault();
        }
        public List<long> FindIdsNotaFiscalPagamentoPreAprovadas()
        {
            return Queryable().Where(x => x.NotaFiscalPagamento.Situacao == SituacaoNotaFiscal.PreAprovado).Select(x => x.NotaFiscalPagamento.Id).ToList();
        }
    }
}
