using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Models
{
    public class PedidoDTO
    {
        public string Mandante { get; set; }

        public string NumeroRequisicaoCompra { get; set; }

        public string NumeroItemRequisicaoCompra { get; set; }

        public string NumeroDocumentoCompra { get; set; }

        public string NumeroItemDocumentoCompra { get; set; }

        public DateTime Data { get; set; }
        public string CodigoLiberacaoDocumentoCompra { get; set; }

        public string LinhaTexto { get; set; }

        public string Status { get; set; }
    }
}
