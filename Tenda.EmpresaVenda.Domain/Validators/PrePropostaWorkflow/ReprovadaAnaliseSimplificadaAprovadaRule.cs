using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class ReprovadaAnaliseSimplificadaAprovadaRule : PrePropostaWorkflowBase
    {
        
        public ReprovadaAnaliseSimplificadaAprovadaRule()
        {
            Origem = SituacaoProposta.Reprovada;
            Destino = SituacaoProposta.AnaliseSimplificadaAprovada;
            Verbo = GlobalMessages.Aprovar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
