using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class CondicionadaAnaliseCompletaAprovadaRule : PrePropostaWorkflowBase
    {
        public CondicionadaAnaliseCompletaAprovadaRule()
        {
            Origem = SituacaoProposta.Condicionada;
            Destino = SituacaoProposta.AnaliseCompletaAprovada;
            Verbo = GlobalMessages.Aprovar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
