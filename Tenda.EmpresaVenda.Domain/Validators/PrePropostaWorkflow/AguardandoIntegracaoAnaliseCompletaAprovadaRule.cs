using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AguardandoIntegracaoAnaliseCompletaAprovadaRule:PrePropostaWorkflowBase
    {
        public AguardandoIntegracaoAnaliseCompletaAprovadaRule()
        {
            Origem = SituacaoProposta.AguardandoIntegracao;
            Destino = SituacaoProposta.AnaliseCompletaAprovada;
            Verbo = GlobalMessages.Retroceder;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return preProposta.SituacaoProposta == SituacaoProposta.AguardandoIntegracao;
        }
    }
}
