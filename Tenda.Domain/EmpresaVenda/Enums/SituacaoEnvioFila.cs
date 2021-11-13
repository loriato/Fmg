using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoEnvioFila
    {
        [Display(Name = "SituacaoEnvioFila_Pendente", ResourceType = typeof(GlobalMessages))]
        Pendente = 0,
        [Display(Name = "SituacaoEnvioFila_EnviadoComSucesso", ResourceType = typeof(GlobalMessages))]
        EnviadoComSucesso = 1,
        [Display(Name = "SituacaoEnvioFila_TentativaComErro", ResourceType = typeof(GlobalMessages))]
        TentativaComErro = 2,
        [Display(Name = "SituacaoEnvioFila_CanceladoExcessoTentativas", ResourceType = typeof(GlobalMessages))]
        CanceladoExcessoTentativas = 3,
        [Display(Name = "SituacaoEnvioFila_Cancelado", ResourceType = typeof(GlobalMessages))]
        Cancelado = 4
    }
}
