using Europa.Extensions;
using Tenda.EmpresaVenda.ApiService.Models.BreveLancamento;

namespace Tenda.EmpresaVenda.ApiService.Models.EnderecoBreveLancamento
{
    public class EnderecoBreveLancamentoDTO
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
        public BreveLancamentoDto BreveLancamento { get; set; }

        public EnderecoBreveLancamentoDTO FromDomain(Tenda.Domain.EmpresaVenda.Models.EnderecoBreveLancamento model)
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
            BreveLancamento = model.BreveLancamento.IsEmpty() ? null : new BreveLancamentoDto().FromDomain(model.BreveLancamento);
            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.EnderecoBreveLancamento ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.EnderecoBreveLancamento();
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
