using Europa.Extensions;
using Tenda.Domain.EmpresaVenda.Models;
//using Tenda.EmpresaVenda.ApiService.Models.Empreendimento;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.EnderecoEmpreendimento
{
    public class EnderecoEmpreendimentoDTO
    {
        public long Id { get; set; }
        public string Cidade { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string Nome { get; set; }
        public Empreendimento.EmpreendimentoDto Empreendimento { get; set; }

        public EnderecoEmpreendimentoDTO FromDomain(Tenda.Domain.EmpresaVenda.Models.EnderecoEmpreendimento model)
        {
            Id = model.Id;
            Cidade = model.Cidade;
            Logradouro = model.Logradouro;
            Bairro = model.Bairro;
            Numero = model.Numero;
            Cep = model.Cep;
            Complemento = model.Complemento;
            Estado = model.Estado;
            Pais = model.Pais;
            Nome = Logradouro + ", " + Numero + ", " + Bairro + ", " + Cidade + " - " + Estado;
            Empreendimento = new Empreendimento.EmpreendimentoDto() { Id = model.Empreendimento.Id, Nome = model.Empreendimento.Nome };
            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.EnderecoEmpreendimento ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.EnderecoEmpreendimento();
            model.Id = Id;
            model.Cidade = Cidade;
            model.Logradouro = Logradouro;
            model.Bairro = Bairro;
            model.Numero = Numero;
            model.Cep = Cep;
            model.Complemento = Complemento;
            model.Estado = Estado;
            model.Pais = Pais;
            return model;
        }
    }
}
