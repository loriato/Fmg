using System;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class EmpreendimentoDTO
    {
        public long Id { get; set; }
        public string NomeRegional { get; set; }
        public string Divisao { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string RegistroIncorporacao { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public DateTime? DataLancamento { get; set; }
        public DateTime? PrevisaoEntrega { get; set; }
        public DateTime? DataEntrega { get; set; }
        public bool DisponivelCatalogo { get; set; }
        public bool DisponivelVenda { get; set; }
        public string CepEmpresa { get; set; }
        public string LogradouroEmpresa { get; set; }
        public string NumeroEmpresa { get; set; }
        public string ComplementoEmpresa { get; set; }
        public string BairroEmpresa { get; set; }
        public string CidadeEmpresa { get; set; }
        public string EstadoEmpresa { get; set; }
        public long IdEmpresa { get; set; }
    }
}