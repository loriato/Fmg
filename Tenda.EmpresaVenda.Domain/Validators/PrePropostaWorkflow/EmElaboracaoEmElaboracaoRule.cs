using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmElaboracaoEmElaboracaoRule : PrePropostaWorkflowBase
    {
        public EmElaboracaoEmElaboracaoRule()
        {
            Origem = SituacaoProposta.EmElaboracao;
            Destino = SituacaoProposta.EmElaboracao;
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
