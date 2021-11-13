using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class NumeroPedidoSap : BaseEntity
    {
        public virtual string Mandante { get; set; }

        public virtual string NumeroRequisicaoCompra { get; set; }

        public virtual string NumeroItemRequisicaoCompra { get; set; }

        public virtual string NumeroDocumentoCompra { get; set; }

        public virtual string NumeroItemDocumentoCompra { get; set; }

        public virtual DateTime Data { get; set; }
        public virtual string CodigoLiberacaoDocumentoCompra { get; set; }

        public virtual string LinhaTexto { get; set; }

        public virtual string Status { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
