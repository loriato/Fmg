using Tenda.Domain.Tenda.CEP;

namespace Tenda.Domain.Core.Services.Models
{
    /*  
     *  This DTO is retorno www.viacep.com.br
     *  Exemplo: http://viacep.com.br/ws/29102380/json/
     *  Os atributos complemento, unidade, ibge e gia estão vazios para igualar ao serviço
    */
    public class CepDTO
    {
        public string logradouro { get; set; }
        public string logradouroAbrev { get; set; }
        public string complemento { get { return ""; } }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string tipo { get; set; }
        public string cep { get; set; }
        public string unidade { get { return ""; } }
        public string ibge { get { return ""; } }
        public string gia { get { return ""; } }

        public CepDTO()
        {
        }

        public CepDTO(endereco endereco)
        {
            logradouro = endereco.tipo + ' ' + endereco.logradouro;
            logradouroAbrev = endereco.logradouroAbrev;
            if (logradouroAbrev != null)
            {
                logradouroAbrev = logradouroAbrev.Trim();
            }
            tipo = endereco.tipo;
            bairro = endereco.bairro;
            localidade = endereco.cidade;
            uf = endereco.uf;
            cep = endereco.cep;
        }
    }
}