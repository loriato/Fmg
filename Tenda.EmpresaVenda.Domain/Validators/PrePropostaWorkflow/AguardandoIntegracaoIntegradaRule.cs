using Europa.Commons;
using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    /*Este passo foi criado para solucionar o problema causado na hora de mudar a situação
     no momento da integração.
     A integração deve ser revista para que esta não gerer mais problemas futuros*/
    public class AguardandoIntegracaoIntegradaRule : PrePropostaWorkflowBase
    {
        private PrePropostaService _prePropostaService { get; set; }
        public AguardandoIntegracaoIntegradaRule()
        {
            Origem = SituacaoProposta.AguardandoIntegracao;
            Destino = SituacaoProposta.Integrada;
            Verbo = GlobalMessages.IntegrarPreProposta;
        }

        /*É preciso verificar os passos da integração com o SUAT,
         bem como analisar os métodos e ajustá-los para inclusão no service.
         */
        public override bool Validate(PreProposta preProposta)
        {
            var bre = new BusinessRuleException();
            var validationResult = new IntegracaoPrePropostaValidator().Validate(preProposta);
            bre.WithFluentValidation(validationResult).ThrowIfHasError();
            return true;
        }
    }
}
