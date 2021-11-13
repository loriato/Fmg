using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class DocumentoProponenteRuleMachinePreProposta : BaseEntity
    {
        public virtual RuleMachinePreProposta RuleMachinePreProposta { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual bool Obrigatorio { get; set; }
        public virtual bool ObrigatorioPortal { get; set; }
        public virtual bool ObrigatorioHouse { get; set; }



        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
