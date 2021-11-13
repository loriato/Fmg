using Europa.Commons;
using Europa.Resources;
using FluentValidation.Results;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class IntegradaAguardandoIntegracaoRule : PrePropostaWorkflowBase
    {
        public IntegradaAguardandoIntegracaoRule()
        {
            Origem = SituacaoProposta.Integrada;
            Destino = SituacaoProposta.AguardandoIntegracao;
        }
        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
