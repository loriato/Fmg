using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoHierarquiaHouse
    {
        [Display(Name = "SituacaoHierarquiaHouse_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoHierarquiaHouse_Inativo", ResourceType = typeof(GlobalMessages))]
        Inativo = 2
    }
}
