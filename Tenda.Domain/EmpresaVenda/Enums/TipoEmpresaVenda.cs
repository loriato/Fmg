using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoEmpresaVenda
    {
        [Display(Name = "TipoEmpresaVenda_EmpresaVenda", ResourceType = typeof(GlobalMessages))]
        EmpresaVenda = 1,
        [Display(Name = "TipoEmpresaVenda_Loja", ResourceType = typeof(GlobalMessages))]
        Loja = 2
    }
}