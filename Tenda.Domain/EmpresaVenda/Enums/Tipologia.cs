using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum Tipologia
    {
        [Display(Name = "Tipologia_PNE", ResourceType = typeof(GlobalMessages))]
        PNE = 1,
        [Display(Name = "Tipologia_FaixaUmMeio", ResourceType = typeof(GlobalMessages))]
        FaixaUmMeio = 2,
        [Display(Name = "Tipologia_FaixaDois", ResourceType = typeof(GlobalMessages))]
        FaixaDois = 3
    }
}
