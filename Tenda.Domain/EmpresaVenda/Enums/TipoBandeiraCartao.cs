using Europa.Resources;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    public enum TipoBandeiraCartao
    {
        [Display(Name = "TipoBandeiraCartao_Mastercard", ResourceType = typeof(GlobalMessages))]
        MasterCard = 1,
        [Display(Name = "TipoBandeiraCartao_Visa", ResourceType = typeof(GlobalMessages))]
        Visa = 2,
        [Display(Name = "TipoBandeiraCartao_Elo", ResourceType = typeof(GlobalMessages))]
        Elo = 3,
        [Display(Name = "TipoBandeiraCartao_AmericanExpress", ResourceType = typeof(GlobalMessages))]
        AmericanExpress = 4,
        [Display(Name = "TipoBandeiraCartao_DinersClub", ResourceType = typeof(GlobalMessages))]
        DinersClub = 5,
        [Display(Name = "TipoBandeiraCartao_Hipercard", ResourceType = typeof(GlobalMessages))]
        Hipercard = 6,
        [Display(Name = "TipoBandeiraCartao_Outros", ResourceType = typeof(GlobalMessages))]
        Outros = 7
    }
}
