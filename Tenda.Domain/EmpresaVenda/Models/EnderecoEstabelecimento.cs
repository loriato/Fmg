using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnderecoEstabelecimento : Endereco
    {
        public virtual Empreendimento Empreendimento { get; set; }
    }
}
