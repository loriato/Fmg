using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseSimplificadaAguardandoAuditoriaRule : PrePropostaWorkflowBase
    {
        
        public EmAnaliseSimplificadaAguardandoAuditoriaRule()
        {
            Origem = SituacaoProposta.EmAnaliseSimplificada;
            Destino = SituacaoProposta.AguardandoAuditoria;
            Verbo = GlobalMessages.EmAuditoria;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
