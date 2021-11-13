using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoRenda
    {
        [Display(Name = "TipoRenda_Informal", ResourceType = typeof(GlobalMessages))]
        Informal = 1,
        [Display(Name = "TipoRenda_Formal", ResourceType = typeof(GlobalMessages))]
        Formal = 2,
        [Display(Name = "TipoRenda_Mista", ResourceType = typeof(GlobalMessages))]
        Mista = 3
    }
}
