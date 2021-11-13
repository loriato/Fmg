using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class FiltroIntegracaoSuatDto
    {
        public long IdPreProposta { get; set; }
        public long IdUnidadeSuat { get; set; }
        public string IdentificadorUnidadeSuat { get; set; }
        public long IdTorre { get; set; }
        public string NomeTorre { get; set; }
    }
}
