using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AguardandoAuditoriaEmAnaliseSimplificadaRule : PrePropostaWorkflowBase
    {
        
        public AguardandoAuditoriaEmAnaliseSimplificadaRule()
        {
            Origem = SituacaoProposta.AguardandoAuditoria;
            Destino = SituacaoProposta.EmAnaliseSimplificada;
            Verbo = GlobalMessages.Retornar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
