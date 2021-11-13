using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Security.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoParametro
    {
        [Display(Name = "Generico", ResourceType = typeof(GlobalMessages))]
        Generico = 1
    }
}
