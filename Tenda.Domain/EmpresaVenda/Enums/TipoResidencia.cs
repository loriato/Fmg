using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoResidencia
    {
        [Display(Name = "TipoResidencia_Propria", ResourceType = typeof(GlobalMessages))]
        Propria = 1,
        [Display(Name = "TipoResidencia_Alugada", ResourceType = typeof(GlobalMessages))]
        Alugada = 2,
        [Display(Name = "TipoResidencia_Amigos", ResourceType = typeof(GlobalMessages))]
        Amigos = 3,
        [Display(Name = "TipoResidencia_Familia", ResourceType = typeof(GlobalMessages))]
        Familia = 4
    }
}
