using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoOrigem
    {
        [Display(Name = "TipoOrigem_Manual", ResourceType = typeof(GlobalMessages))]
        Manual = 1,
        [Display(Name = "TipoOrigem_Robo", ResourceType = typeof(GlobalMessages))]
        Robo = 2
    }
}
