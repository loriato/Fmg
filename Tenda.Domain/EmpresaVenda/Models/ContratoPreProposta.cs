using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ContratoPreProposta : BaseEntity
    {
        public virtual PreProposta PreProposta { get; set; }
        public virtual byte[] Contrato { get; set; }
        public virtual long IdContratoSuat { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
