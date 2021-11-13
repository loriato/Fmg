using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnderecoEmpresa : Endereco
    {
        public virtual Cliente Cliente { get; set; }
    }
}
