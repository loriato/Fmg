using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AguardandoAnaliseCompletaEmAnaliseCompletaRule:PrePropostaWorkflowBase
    {
        public AguardandoAnaliseCompletaEmAnaliseCompletaRule()
        {
            Origem = SituacaoProposta.AguardandoAnaliseCompleta;
            Destino = SituacaoProposta.EmAnaliseCompleta;
            Verbo = GlobalMessages.Analisar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
