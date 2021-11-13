using Europa.Data.Model;

namespace Tenda.Domain.Core.Models
{
    public class Endereco : BaseEntity
    {
        public virtual string Cidade { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Pais { get; set; }

        public override string ChaveCandidata()
        {
            return Logradouro + ", " + Numero + ", " + Bairro + ", " + Cidade + " - " + Estado;
        }
    }
}
