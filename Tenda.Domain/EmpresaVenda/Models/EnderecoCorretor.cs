using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class EnderecoCorretor : Endereco
    {
        public virtual Corretor Corretor { get; set; }

        public override string ChaveCandidata()
        {
            return Logradouro + ", " + Numero + ", " + Bairro + ", " + Cidade + " - " + Estado;
        }
    }
}
