using Europa.Commons;
using FluentValidation.Results;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseSimplificadaAprovadaAguardandoIntegracaoRule : PrePropostaWorkflowBase
    {
        
        public AnaliseSimplificadaAprovadaAguardandoIntegracaoRule()
        {
            Origem = SituacaoProposta.AnaliseSimplificadaAprovada;
            Destino = SituacaoProposta.AguardandoIntegracao;
        }

        public override bool Validate(PreProposta preProposta)
        {
            BusinessRuleException bre = new BusinessRuleException();

            // Realiza as validações da Pré Proposta
            ValidationResult anprResult = new FinalizacaoPrePropostaValidator().Validate(preProposta);

            // Verifica se existe algum erro e retorna exceção se necessário
            bre.WithFluentValidation(anprResult);

            bre.ThrowIfHasError();

            return true;
        }
    }
}
