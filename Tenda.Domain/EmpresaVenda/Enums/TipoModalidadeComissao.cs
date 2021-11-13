using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoModalidadeComissao
    {
        [Display(Name = "TipoModalidadeComissao_Fixa", ResourceType = typeof(GlobalMessages))]
        Fixa = 1,
        [Display(Name = "TipoModalidadeComissao_Nominal", ResourceType = typeof(GlobalMessages))]
        Nominal = 2
    }
}
