using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseSimplificadaAprovadaEmAnaliseSimplificadaRule : PrePropostaWorkflowBase
    {
        

        public AnaliseSimplificadaAprovadaEmAnaliseSimplificadaRule()
        {
            Origem = SituacaoProposta.AnaliseSimplificadaAprovada;
            Destino = SituacaoProposta.EmAnaliseSimplificada;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
