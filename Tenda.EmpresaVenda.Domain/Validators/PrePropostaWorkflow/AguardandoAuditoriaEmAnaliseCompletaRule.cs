using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AguardandoAuditoriaEmAnaliseCompletaRule:PrePropostaWorkflowBase
    {
        public AguardandoAuditoriaEmAnaliseCompletaRule()
        {
            Origem = SituacaoProposta.AguardandoAuditoria;
            Destino = SituacaoProposta.EmAnaliseCompleta;
            Verbo = GlobalMessages.Retornar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
