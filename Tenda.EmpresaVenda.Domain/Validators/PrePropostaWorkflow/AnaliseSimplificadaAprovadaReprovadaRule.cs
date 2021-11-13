using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseSimplificadaAprovadaReprovadaRule : PrePropostaWorkflowBase
    {
        
        public AnaliseSimplificadaAprovadaReprovadaRule()
        {
            Origem = SituacaoProposta.AnaliseSimplificadaAprovada;
            Destino = SituacaoProposta.Reprovada;
        }

        public override bool Validate(PreProposta preProposta)
        {
            if (preProposta.StatusSicaqPrevio != StatusSicaq.Reprovado)
            {
                return false;
            }

            return true;
        }
    }
}
