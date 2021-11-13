using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class ReprovadaAnaliseCompletaAprovadaRule : PrePropostaWorkflowBase
    {
        public ReprovadaAnaliseCompletaAprovadaRule()
        {
            Origem = SituacaoProposta.Reprovada;
            Destino = SituacaoProposta.AnaliseCompletaAprovada;
            Verbo = GlobalMessages.Aprovar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
