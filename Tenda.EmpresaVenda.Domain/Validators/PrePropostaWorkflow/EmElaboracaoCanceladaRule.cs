using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmElaboracaoCanceladaRule : PrePropostaWorkflowBase
    {
        public EmElaboracaoCanceladaRule()
        {
            Origem = SituacaoProposta.EmElaboracao;
            Destino = SituacaoProposta.Cancelada;
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
