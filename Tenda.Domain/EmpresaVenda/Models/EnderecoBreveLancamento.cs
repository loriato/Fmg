using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnderecoBreveLancamento : Endereco
    {
        public virtual BreveLancamento BreveLancamento { get; set; }
    }
}
