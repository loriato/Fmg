using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class OcorrenciasMidasRepository : NHibernateRepository<OcorrenciasMidas>
    {
        public NotaFiscalPagamentoOcorrenciaRepository _notaFiscalPagamentoOcorrenciaRepository { get; set; }
        public OcorrenciasMidas findByIdOcorrencia(long idOcorrencia)
        {
            var query = Queryable().Where(x => x.OccurenceId == idOcorrencia).FirstOrDefault();
            return query;

        }
    

        public List<OcorrenciasMidas> ListarOcorrenciasCommissionavies()
        {
            return Queryable().Where(x => x.CanBeCommissioned == true).ToList();
        }

        public OcorrenciasMidas ListarOcorrenciaMatch(string numeroNota, string cnpjEv, string cnpjProduto)
        {
            return Queryable().Where(x => x.TaxIdProvider.Equals(cnpjEv) && x.TaxIdTaker.Equals(cnpjProduto) && x.Document.Number.Equals(numeroNota))
                .FirstOrDefault();
        }

        public bool MatchPagamentoOcorrencias(string cnpjEv, string cnpjProd, NotaFiscalPagamento notaFiscalPagamento)
        {
            OcorrenciasMidas ocorrencia = ListarOcorrenciaMatch(notaFiscalPagamento.NotaFiscal, cnpjEv, cnpjProd);

            var crz = new NotaFiscalPagamentoOcorrencia();
            if (ocorrencia != null)
            {
                var cruzamentoExistente = _notaFiscalPagamentoOcorrenciaRepository.Queryable()
                .Where(x => x.NotaFiscalPagamento.Id == notaFiscalPagamento.Id || x.Ocorrencia.Id == ocorrencia.Id)
                .Any();

                if (cruzamentoExistente)
                {
                    return true;
                }

                crz.NotaFiscalPagamento = notaFiscalPagamento;
                crz.Ocorrencia = ocorrencia;
                _notaFiscalPagamentoOcorrenciaRepository.Save(crz);
                return true;
            }

            return false;
        }
    }
}
