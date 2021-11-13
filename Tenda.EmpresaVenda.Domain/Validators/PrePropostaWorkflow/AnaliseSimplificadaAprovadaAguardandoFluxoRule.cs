using Europa.Commons;
using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseSimplificadaAprovadaAguardandoFluxoRule : PrePropostaWorkflowBase
    {
        
        public AnaliseSimplificadaAprovadaAguardandoFluxoRule()
        {
            Origem = SituacaoProposta.AnaliseSimplificadaAprovada;
            Destino = SituacaoProposta.AguardandoFluxo;
            Verbo = GlobalMessages.Aguardando;
        }

        public override bool Validate(PreProposta preProposta)
        {
            var bre = new BusinessRuleException();

            var result = new AlteracaoSicaqPrevioPrePropostaValidator().Validate(preProposta);
            bre.WithFluentValidation(result);

            bre.ThrowIfHasError();

            Notificacao notificacao = new Notificacao()
            {
                Titulo = string.Format(GlobalMessages.Notificacao_AguardandoFluxo_Titulo, preProposta.Codigo, preProposta.Cliente.NomeCompleto),
                Conteudo = GlobalMessages.Notificacao_AguardandoFluxo_Conteudo,
                Usuario = preProposta.Corretor.Usuario,
                EmpresaVenda = preProposta.EmpresaVenda,
                DestinoNotificacao = DestinoNotificacao.Portal,
            };

            _notificacaoRepository.Save(notificacao);

            return true;
        }
    }
}
