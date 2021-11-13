using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseSimplificadaAguardandoAnaliseSimplificadaRule : PrePropostaWorkflowBase
    {        
        public EmAnaliseSimplificadaAguardandoAnaliseSimplificadaRule()
        {
            Origem = SituacaoProposta.EmAnaliseSimplificada;
            Destino = SituacaoProposta.AguardandoAnaliseSimplificada;
            Verbo = GlobalMessages.Retornar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
