using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoEscolaridade
    {
        [Display(Name = "TipoEscolaridade_FundamentalIncompleto", ResourceType = typeof(GlobalMessages))]
        FundamentalIncompleto = 1,
        [Display(Name = "TipoEscolaridade_FuncamentalCompleto", ResourceType = typeof(GlobalMessages))]
        FundamentalCompleto = 2,
        [Display(Name = "TipoEscolaridade_MedioIncompleto", ResourceType = typeof(GlobalMessages))]
        MedioIncompleto = 3,
        [Display(Name = "TipoEscolaridade_MedioCompleto", ResourceType = typeof(GlobalMessages))]
        MedioCompleto = 4,
        [Display(Name = "TipoEscolaridade_SuperiorIncompleto", ResourceType = typeof(GlobalMessages))]
        SuperiorIncompleto = 5,
        [Display(Name = "TipoEscolaridade_SuperiorCompleto", ResourceType = typeof(GlobalMessages))]
        SuperiorCompleto = 6,
        [Display(Name = "TipoEscolaridade_Mestrado", ResourceType = typeof(GlobalMessages))]
        Mestrado = 7,
        [Display(Name = "TipoEscolaridade_Doutorado", ResourceType = typeof(GlobalMessages))]
        Doutora = 8
    }
}
