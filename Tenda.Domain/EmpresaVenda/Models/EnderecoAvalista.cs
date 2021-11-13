using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnderecoAvalista : Endereco
    {
        public virtual Avalista Avalista { get; set; }
    }
}
