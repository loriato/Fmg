using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class CondicionadaAnaliseSimplificadaAprovadaRule : PrePropostaWorkflowBase
    {
        
        public CondicionadaAnaliseSimplificadaAprovadaRule()
        {
            Origem = SituacaoProposta.Condicionada;
            Destino = SituacaoProposta.AnaliseSimplificadaAprovada;
            Verbo = GlobalMessages.Aprovar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
