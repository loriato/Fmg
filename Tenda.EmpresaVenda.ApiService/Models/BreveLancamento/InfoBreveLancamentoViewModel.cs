using System.Collections.Generic;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoBreveLancamento;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoEmpreendimento;

namespace Tenda.EmpresaVenda.ApiService.Models.BreveLancamento
{
    public class InfoBreveLancamentoViewModel
    {
        public EnderecoBreveLancamentoDTO Endereco { get; set; }
        public List<ViewArquivoBreveLancamento> ArquivoBreveLancamento { get; set; }
        public List<ViewArquivoEmpreendimento> ArquivoEmpreendimento { get; set; }
        public List<ArquivoDto> ListaArquivo { get; set; }
        public EnderecoEmpreendimentoDTO EnderecoEmpreendimento { get; set; }
    }
}