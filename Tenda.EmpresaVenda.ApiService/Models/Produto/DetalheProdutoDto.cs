using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;

namespace Tenda.EmpresaVenda.ApiService.Models.Produto
{
    public class DetalheProdutoDto
    {
        public long Id { get; set; }
        public long? IdEmpreendimento { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Pais { get; set; }
        public string Cep { get; set; }
        public bool EmpreendimentoVerificado { get; set; }
        public string Informacoes { get; set; }
        public string FichaTecnica { get; set; }

        public List<ArquivoDto> Arquivos { get; set; }

        public void FromDomain(ViewProduto model)
        {
            IdEmpreendimento = model.IdEmpreendimento;
            Id = model.Id;
            Nome = model.Nome;
            Cidade = model.Cidade;
            Estado = model.Estado;
            Bairro = model.Bairro;
            Logradouro = model.Logradouro;
            Numero = model.Numero;
            EmpreendimentoVerificado = model.EmpreendimentoVerificado;
            Informacoes = model.Informacoes;
            FichaTecnica = model.FichaTecnica;
        }

        public string FormatarEndereco()
        {
            return string.Join(", ",
                new string[] {Logradouro, Numero, Bairro, Cidade, Estado, Pais, Cep}.Where(c => !string.IsNullOrEmpty(c)));
        }
       
    }
}
