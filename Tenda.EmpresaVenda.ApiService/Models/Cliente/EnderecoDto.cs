using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class EnderecoDto:EntityDto
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        public void FromDomain(Tenda.Domain.Core.Models.Endereco endereco)
        {
            Id = endereco.Id;
            Cep = endereco.Cep;
            Logradouro = endereco.Logradouro;
            Numero = endereco.Numero;
            Complemento = endereco.Complemento;
            Bairro = endereco.Bairro;
            Estado = endereco.Estado;
            Cidade = endereco.Cidade;
        }
    }
}
