using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoOrigemProposta
    {
        [Display(Name = "TipoOrigemProposta_EVS", ResourceType = typeof(GlobalMessages))]
        EVS = 1,
        [Display(Name = "TipoOrigemProposta_SUAT", ResourceType = typeof(GlobalMessages))]
        SUAT = 2
    }
}
