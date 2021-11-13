using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class NotaFiscalPagamentoOcorrencia : BaseEntity
    {
        public virtual OcorrenciasMidas Ocorrencia { get; set; }
        public virtual NotaFiscalPagamento NotaFiscalPagamento  { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
