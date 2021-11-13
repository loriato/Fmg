using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class NotaFiscalPagamento : BaseEntity
    {
        public virtual string NotaFiscal { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual SituacaoNotaFiscal Situacao { get; set; }
        public virtual string Motivo { get; set; }
        public virtual DateTime? DataPedido { get; set; }
        public virtual DateTime? DataEnviado { get; set; }
        public virtual DateTime? DataRecebido { get; set; }
        public virtual DateTime? DataAprovado { get; set; }
        public virtual DateTime? DataReprovado { get; set; }
        public virtual int RevisaoNF { get; set; }
        public virtual string Chave { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
