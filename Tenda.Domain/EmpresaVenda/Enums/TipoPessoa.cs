using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoPessoa
    {
        [Display(Name = "TipoPessoa_Fisica", ResourceType = typeof(GlobalMessages))]
        Fisica = 1,
        [Display(Name = "TipoPessoa_Juridica", ResourceType = typeof(GlobalMessages))]
        Juridica = 2
    }
}
