using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class FiltroFornecedorDTO
    {
        public virtual string CodigoFornecedor { get; set; }
        public virtual List<string> Estados { get; set; }
    }
}
