using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseSimplificadaAprovadaSICAQComErroRule : PrePropostaWorkflowBase
    {
        
        public AnaliseSimplificadaAprovadaSICAQComErroRule()
        {
            Origem = SituacaoProposta.AnaliseSimplificadaAprovada;
            Destino = SituacaoProposta.SICAQComErro;
        }

        public override bool Validate(PreProposta preProposta)
        {
            if (preProposta.StatusSicaqPrevio != StatusSicaq.SICAQComErro)
            {
                return false;
            }

            return true;
        }
    }
}
