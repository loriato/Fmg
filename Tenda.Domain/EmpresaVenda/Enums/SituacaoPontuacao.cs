using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoPontuacao
    {
        [Display(Name = "SituacaoPontuacao_Indisponivel", ResourceType = typeof(GlobalMessages))]
        Indisponivel = 0,
        [Display(Name = "SituacaoPontuacao_Disponivel", ResourceType = typeof(GlobalMessages))]
        Disponivel = 1
    }
}
