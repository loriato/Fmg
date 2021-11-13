using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnderecoCliente : Endereco
    {
        public virtual Cliente Cliente { get; set; }
    }
}
