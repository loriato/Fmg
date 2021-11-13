using Europa.Extensions;
using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Loja
{
    public class LojaPortalDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string NomeComercial { get; set; }
        public string PessoaContato { get; set; }
        public string TelefoneContato { get; set; }
        public string Cidade { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Estado { get; set; }
        public long IdCentralVenda { get; set; }
        public string NomeCentralVenda { get; set; }
        public List<long> idsRegionais { get; set; }
        public virtual TipoSimNao ConsiderarUF { get; set; }
        public Situacao Situacao { get; set; }


        public LojaPortalDto FromDomain(Tenda.Domain.EmpresaVenda.Models.Views.ViewLojasPortal model)
        {
            Id = model.Id;
            Nome = model.Nome;
            NomeComercial = model.NomeComercial;
            PessoaContato = model.PessoaContato;
            TelefoneContato = model.TelefoneContato;
            Cidade = model.Cidade;
            Logradouro = model.Logradouro;
            Bairro = model.Bairro;
            Cep = model.Cep;
            Numero = model.Numero;
            Complemento = model.Complemento;
            Estado = model.Estado;
            IdCentralVenda = model.IdCentralVenda;
            NomeCentralVenda = model.NomeCentralVenda;
            Situacao = model.Situacao;
            ConsiderarUF = model.ConsiderarUF;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();

            
                model.Loja = new Tenda.Domain.EmpresaVenda.Models.Loja();
                model.Id = Id;
                model.NomeFantasia = Nome;
                model.RazaoSocial = NomeComercial;
                model.PessoaContato = PessoaContato;
                model.TelefoneContato = TelefoneContato.OnlyNumber();
                model.TipoEmpresaVenda = TipoEmpresaVenda.Loja;
                model.Cidade = Cidade;
                model.Logradouro = Logradouro;
                model.Bairro = Bairro;
                model.Cep = Cep;
                model.Numero = Numero;
                model.Complemento = Complemento;
                model.Estado = Estado;
                model.Loja.Id = IdCentralVenda;
                model.Loja.Nome = NomeCentralVenda;
                model.Situacao = Situacao;
                model.ConsiderarUF = ConsiderarUF;

            return model;
        }
    }
}
