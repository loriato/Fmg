using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class CondicionadaCanceladaRule: PrePropostaWorkflowBase
    {
        public CondicionadaCanceladaRule()
        {
            Origem = SituacaoProposta.Condicionada;
            Destino = SituacaoProposta.Cancelada;
            Verbo = GlobalMessages.Cancelar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
