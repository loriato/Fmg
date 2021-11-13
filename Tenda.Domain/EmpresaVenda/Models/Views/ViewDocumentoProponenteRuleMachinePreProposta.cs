using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewDocumentoProponenteRuleMachinePreProposta : BaseEntity
    {
        public virtual long IdTipoDocumento { get; set; }
        public virtual string NomeTipoDocumento { get; set; }
        public virtual long IdRuleMachinePreProposta { get; set; }
        public virtual SituacaoProposta Origem { get; set; }
        public virtual SituacaoProposta Destino { get; set; }
        public virtual long IdDocumentoRuleMachine { get; set; }
        public virtual bool Obrigatorio { get; set; }
        public virtual bool ObrigatorioPortal { get; set; }
        public virtual bool ObrigatorioHouse { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
