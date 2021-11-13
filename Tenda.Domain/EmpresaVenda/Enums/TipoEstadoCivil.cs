using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoEstadoCivil
    {
        [Display(Name = "TipoEstadoCivil_Solteiro", ResourceType = typeof(GlobalMessages))]
        Solteiro = 1,
        [Display(Name = "TipoEstadoCivil_Casado", ResourceType = typeof(GlobalMessages))]
        Casado = 2,
        [Display(Name = "TipoEstadoCivil_Divorciado", ResourceType = typeof(GlobalMessages))]
        Divorciado = 3,
        [Display(Name = "TipoEstadoCivil_Viuvo", ResourceType = typeof(GlobalMessages))]
        Viuvo = 4,
        [Display(Name = "TipoEstadoCivil_Separado", ResourceType = typeof(GlobalMessages))]
        Separado = 5,
        [Display(Name = "TipoEstadoCivil_UniaoEstavel", ResourceType = typeof(GlobalMessages))]
        UniaoEstavel = 6
    }
}
