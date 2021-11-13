using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseCompletaAguardandoAnaliseCompletaRule : PrePropostaWorkflowBase
    {        
        public EmAnaliseCompletaAguardandoAnaliseCompletaRule()
        {
            Origem = SituacaoProposta.EmAnaliseCompleta;
            Destino = SituacaoProposta.AguardandoAnaliseCompleta;
            Verbo = GlobalMessages.Retornar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
