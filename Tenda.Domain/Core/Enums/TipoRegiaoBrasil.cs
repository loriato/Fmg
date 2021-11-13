using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Core.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoRegiaoBrasil
    {
        [Display(Name = "TipoRegiaoBrasil_Sul", ResourceType = typeof(GlobalMessages))]
        Sul = 1,
        [Display(Name = "TipoRegiaoBrasil_Sudeste", ResourceType = typeof(GlobalMessages))]
        Sudeste = 2,
        [Display(Name = "TipoRegiaoBrasil_CentroOeste", ResourceType = typeof(GlobalMessages))]
        CentroOeste = 3,
        [Display(Name = "TipoRegiaoBrasil_Norte", ResourceType = typeof(GlobalMessages))]
        Norte = 4,
        [Display(Name = "TipoRegiaoBrasil_Nordeste", ResourceType = typeof(GlobalMessages))]
        Nordeste = 5,
        [Display(Name = "TipoRegiaoBrasil_Indiferente", ResourceType = typeof(GlobalMessages))]
        Indiferente = 6,
    }
}