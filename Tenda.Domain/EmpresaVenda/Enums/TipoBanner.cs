using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoBanner
    {
        [Display(Name = "TipoBanner_ModeloNegocio", ResourceType = typeof(GlobalMessages))]
        ModeloNegocio = 1,
        [Display(Name = "TipoBanner_SomenteInformacao", ResourceType = typeof(GlobalMessages))]
        SomenteInformacao = 2
    }
}