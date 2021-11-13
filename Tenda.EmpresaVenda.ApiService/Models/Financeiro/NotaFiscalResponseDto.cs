using System.Collections.Generic;

namespace Tenda.EmpresaVenda.ApiService.Models.Financeiro
{
    public class NotaFiscalResponseDto
    {
        public virtual bool Sucesso { get; set; }
        public virtual List<string> Mensagens { get; set; }
    }
}
