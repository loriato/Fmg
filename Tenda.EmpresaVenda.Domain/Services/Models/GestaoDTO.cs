using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class GestaoDTO
    {
        public DateTime? ReferenciaDe { get; set; }
        public DateTime? ReferenciaAte { get; set; }
        public long? idTipoCusto { get; set; }
        public long? idClassificacao { get; set; }
        public long? idFornecedor { get; set; }
        public long? idEmpresaVenda { get; set; }
        public long? idPontoVenda { get; set; }
        public long? idCentroCusto { get; set; }
    }
}
