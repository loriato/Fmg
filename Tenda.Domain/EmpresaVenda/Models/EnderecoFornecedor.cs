using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnderecoFornecedor : Endereco
    {
        public virtual string CodigoFornecedor{get;set;}
        public virtual string RazaoSocial { get; set; }
        public virtual string Cnpj { get; set; }

    }
}
