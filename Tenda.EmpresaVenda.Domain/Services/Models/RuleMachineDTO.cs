namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class RuleMachineDTO
    {
        public virtual long IdTipoDocumento { get; set; }
        public virtual string NomeTipoDocumento { get; set; }
        public virtual long IdRuleMachinePreProposta { get; set; }
        public virtual long IdDocumentoRuleMachine { get; set; }
        public virtual bool Obrigatorio { get; set; }
        public virtual bool ObrigatorioPortal { get; set; }
        public virtual bool ObrigatorioHouse { get; set; }
    }
}
