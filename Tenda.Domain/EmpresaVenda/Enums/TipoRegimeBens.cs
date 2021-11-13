using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoRegimeBens
    {
        [Display(Name = "TipoRegimeBens_ComunhaoParcialDeBens", ResourceType = typeof(GlobalMessages))]
        ComunhaoParcialDeBens = 1,
        [Display(Name = "TipoRegimeBens_ComunhaoUniversalDeBens", ResourceType = typeof(GlobalMessages))]
        ComunhaoUniversalDeBens = 2,
        [Display(Name = "TipoRegimeBens_ParticipacaoFinalNosAquestos", ResourceType = typeof(GlobalMessages))]
        ParticipacaoFinalNosAquestos = 3,
        [Display(Name = "TipoRegimeBens_SeparacaoTotalDeBens", ResourceType = typeof(GlobalMessages))]
        SeparacaoTotalDeBens = 4,
        [Display(Name = "TipoRegimeBens_Nenhum", ResourceType = typeof(GlobalMessages))]
        Nenhum = 5
    }
}