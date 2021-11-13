using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class PropostaSuatDTO
    {
        public long? CodigoCliente { get; set; }
        public string Proposta { get; set; }
        public string Fase { get; set; }
        public string Sintese { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataRepasse { get; set; }
        public DateTime? DataRegistro { get; set; }
        public string StatusConformidade { get; set; }
        public DateTime? DataConformidade { get; set; }
    }
}
