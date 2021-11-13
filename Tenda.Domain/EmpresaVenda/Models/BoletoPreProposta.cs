using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class BoletoPreProposta : BaseEntity
    {
        public virtual PreProposta PreProposta { get; set; }
        public virtual byte[] Boleto { get; set; }
        public virtual long IdBoletoSuat { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
