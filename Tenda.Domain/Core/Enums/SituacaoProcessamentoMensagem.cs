using Europa.Resources;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Core.Enums
{
    public enum SituacaoProcessamentoMensagem
    {
        [Display(Name = "SituacaoProcessamentoMensagem_NaoDefinido", ResourceType = typeof(GlobalMessages))]
        NaoDefinido = 0,
        [Display(Name = "SituacaoProcessamentoMensagem_Aguardando", ResourceType = typeof(GlobalMessages))]
        Aguardando = 1,
        [Display(Name = "SituacaoProcessamentoMensagem_Enviado", ResourceType = typeof(GlobalMessages))]
        Enviado = 2,
        [Display(Name = "SituacaoProcessamentoMensagem_Falha", ResourceType = typeof(GlobalMessages))]
        Falha = 3,
        [Display(Name = "SituacaoProcessamentoMensagem_CanceladaLimiteErro", ResourceType = typeof(GlobalMessages))]
        CanceladaLimiteErro = 4
    }
}
