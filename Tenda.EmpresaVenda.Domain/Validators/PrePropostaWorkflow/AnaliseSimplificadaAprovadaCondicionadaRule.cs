using Europa.Resources;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class AnaliseSimplificadaAprovadaCondicionadaRule : PrePropostaWorkflowBase
    {
        
        public AnaliseSimplificadaAprovadaCondicionadaRule()
        {
            Origem = SituacaoProposta.AnaliseSimplificadaAprovada;
            Destino = SituacaoProposta.Condicionada;
            Verbo = GlobalMessages.Condicionar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            Notificacao notificacao = new Notificacao()
            {
                Titulo = string.Format(GlobalMessages.Notificacao_Condicionada_Titulo, preProposta.Codigo, preProposta.Cliente.NomeCompleto),
                Conteudo = GlobalMessages.Notificacao_Condicionada_Conteudo,
                Usuario = preProposta.Corretor.Usuario,
                EmpresaVenda = preProposta.EmpresaVenda,
                DestinoNotificacao = DestinoNotificacao.Portal,
            };

            _notificacaoRepository.Save(notificacao);

            return true;

        }

    }
}
