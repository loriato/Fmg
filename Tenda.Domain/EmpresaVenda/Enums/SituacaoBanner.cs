using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoBanner
    {
        [Display(Name = "SituacaoBanner_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoBanner_Inativo", ResourceType = typeof(GlobalMessages))]
        Inativo = 2
    }
}