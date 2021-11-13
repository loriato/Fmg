using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoModalidadeProgramaFidelidade
    {
        [Display(Name = "TipoModalidadeProgramaFidelidade_Fixa", ResourceType = typeof(GlobalMessages))]
        Fixa = 1,
        [Display(Name = "TipoModalidadeProgramaFidelidade_Nominal", ResourceType = typeof(GlobalMessages))]
        Nominal = 2
    }
}
