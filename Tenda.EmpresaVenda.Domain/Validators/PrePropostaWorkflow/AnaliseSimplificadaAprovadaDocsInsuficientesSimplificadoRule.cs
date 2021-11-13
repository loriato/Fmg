using Europa.Resources;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseSimplificadaAprovadaDocsInsuficientesSimplificadoRule : PrePropostaWorkflowBase
    {
        
        public AnaliseSimplificadaAprovadaDocsInsuficientesSimplificadoRule()
        {
            Origem = SituacaoProposta.AnaliseSimplificadaAprovada;
            Destino = SituacaoProposta.DocsInsuficientesSimplificado;
            Verbo = GlobalMessages.Pendenciar;
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
