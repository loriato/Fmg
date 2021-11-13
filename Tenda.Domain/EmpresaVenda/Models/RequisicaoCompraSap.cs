using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class RequisicaoCompraSap : BaseEntity
    {
        public virtual string Numero { get; set; }
        public virtual string Status { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual PropostaSuat Proposta { get; set; }
        public virtual string Texto { get; set; }
        public virtual bool NumeroGerado { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return Numero + " | " + Status + " | " + TipoPagamento.ToString() + " | " + Texto + " | " + NumeroGerado;
        }
    }
}
