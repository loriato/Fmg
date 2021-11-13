
namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class EmpresaVendaTendaDTO
    {
        public string Nome { get; set; }
        public string Regional { get; set; }
        public string Representante { get; set; }
        public string Telefone { get; set; }

        //Endereco
        public string Cidade { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }


        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda model)
        {
            Nome = model.NomeFantasia;
            Regional = model.Loja?.Regional.Nome;
            Representante = model.Corretor?.Nome;
            Telefone = model.Corretor?.Telefone;
            Cidade = model.Cidade;
            Logradouro = model.Logradouro;
            Bairro = model.Bairro;
            Numero = model.Numero;
            Cep = model.Cep;
            Complemento = model.Complemento;
            Estado = model.Estado;
            Pais = model.Pais;
        }
    }
}
