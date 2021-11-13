using System;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroRateioComissaoDTO
    {
        public long IdContratante { get; set; }
        public long IdContratada { get; set; }
        public long IdEmpreendimento { get; set; }
        public DateTime? VigenteEm { get; set; }
    }
}
