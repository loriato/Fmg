using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Fmg.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoCombustivel
    {
        [Display(Name = "TipoCombustivel_GasolinaComum", ResourceType = typeof(GlobalMessages))]
        GasolinaComum = 1,
        [Display(Name = "TipoCombustivel_EtanolComum", ResourceType = typeof(GlobalMessages))]
        EtanolComum = 2,
        [Display(Name = "TipoCombustivel_GasNatural", ResourceType = typeof(GlobalMessages))]
        GasNatural = 3
    }
}
