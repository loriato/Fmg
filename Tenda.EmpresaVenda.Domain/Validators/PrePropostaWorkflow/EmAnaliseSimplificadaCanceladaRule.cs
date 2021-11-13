using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseSimplificadaCanceladaRule : PrePropostaWorkflowBase
    {
        
        public EmAnaliseSimplificadaCanceladaRule()
        {
            Origem = SituacaoProposta.EmAnaliseSimplificada;
            Destino = SituacaoProposta.Cancelada;
            Verbo = GlobalMessages.Cancelar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
