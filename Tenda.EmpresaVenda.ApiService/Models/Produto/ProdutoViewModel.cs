using System;
using System.Collections.Generic;
using Tenda.Domain.Core.Models;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;

namespace Tenda.EmpresaVenda.ApiService.Models.Produto
{
    public class ProdutoViewModel
    {
        public DateTime GeneratedAt { get; set; }
        public string Nome { get; set; }
        public long IdEmpreendimento { get; set; }
        public long IdBreveLancamento { get; set; }
        public string Informacoes { get; set; }
        public Endereco Endereco { get; set; }
        public ArquivoDto ImagemPrincipal { get; set; }
        public List<ArquivoDto> Book { get; set; } = new List<ArquivoDto>();
        public bool DisponivelParaVenda { get; set; }
        public virtual bool VerificarEmpreendimento { get; set; }
        public virtual int? Sequencia { get; set; }

    }
}