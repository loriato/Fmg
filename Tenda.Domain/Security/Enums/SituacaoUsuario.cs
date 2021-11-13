using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Europa.Resources;

namespace Tenda.Domain.Security.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoUsuario
    {
        [Display(Name = "SituacaoUsuario_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoUsuario_Suspenso", ResourceType = typeof(GlobalMessages))]
        Suspenso = 2,
        [Display(Name = "SituacaoUsuario_Cancelado", ResourceType = typeof(GlobalMessages))]
        Cancelado = 3,
        [Display(Name = "SituacaoUsuario_PendenteAprovacao", ResourceType = typeof(GlobalMessages))]
        PendenteAprovacao = 4
    }
}
