using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoFamiliaridade
    {
        [Display(Name = "TipoFamiliaridade_Avo", ResourceType = typeof(GlobalMessages))]
        Avo = 1,
        [Display(Name = "TipoFamiliaridade_Conjuge", ResourceType = typeof(GlobalMessages))]
        Conjuge = 2,
        [Display(Name = "TipoFamiliaridade_Filho", ResourceType = typeof(GlobalMessages))]
        Filho = 3,
        [Display(Name = "TipoFamiliaridade_Irma", ResourceType = typeof(GlobalMessages))]
        Irmao = 4,
        [Display(Name = "TipoFamiliaridade_Mae", ResourceType = typeof(GlobalMessages))]
        Mae = 5,
        [Display(Name = "TipoFamiliaridade_Pai", ResourceType = typeof(GlobalMessages))]
        Pai = 6,
        [Display(Name = "TipoFamiliaridade_Primo", ResourceType = typeof(GlobalMessages))]
        Primo = 7,
        [Display(Name = "TipoFamiliaridade_Tio", ResourceType = typeof(GlobalMessages))]
        Tio = 8
    }
}
