using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class FluxoEnviadoAguardandoFluxoRule : PrePropostaWorkflowBase
    {
        public FluxoEnviadoAguardandoFluxoRule()
        {
            Origem = SituacaoProposta.FluxoEnviado;
            Destino = SituacaoProposta.AguardandoFluxo;
            Verbo = GlobalMessages.Retroceder;
        }

        public override bool Validate(PreProposta preProposta)
        {
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
