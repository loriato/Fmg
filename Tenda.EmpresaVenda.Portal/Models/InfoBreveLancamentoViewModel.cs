using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class InfoBreveLancamentoViewModel
    {
        public EnderecoBreveLancamento Endereco { get; set; }
        public List<ViewArquivoBreveLancamento> ArquivoBreveLancamento { get; set; }
        public List<ViewArquivoEmpreendimento> ArquivoEmpreendimento { get; set; }
        public List<ArquivoDto> ListaArquivo { get; set; }
        public EnderecoEmpreendimento EnderecoEmpreendimento { get; set; }
    }
}