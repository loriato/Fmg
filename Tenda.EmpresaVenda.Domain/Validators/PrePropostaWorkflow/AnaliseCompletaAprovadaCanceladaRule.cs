using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseCompletaAprovadaCanceladaRule: PrePropostaWorkflowBase
    {
        public AnaliseCompletaAprovadaCanceladaRule()
        {
            Origem = SituacaoProposta.AnaliseCompletaAprovada;
            Destino = SituacaoProposta.Cancelada;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
