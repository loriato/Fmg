using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AguardandoAnaliseSimplificadaEmAnaliseSimplificadaRule : PrePropostaWorkflowBase
    {
        
        public AguardandoAnaliseSimplificadaEmAnaliseSimplificadaRule()
        {
            Origem = SituacaoProposta.AguardandoAnaliseSimplificada;
            Destino = SituacaoProposta.EmAnaliseSimplificada;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
            // Implementar a validação dos dados?
            // Preciso validar dados do cliente

            throw new NotImplementedException();
        }
    }
}
