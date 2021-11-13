using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoOrigemSimulacao
    {
        [Display(Name = "TipoOrigemSimulacao_Simulador", ResourceType = typeof(GlobalMessages))]
        Simulador = 1,
        [Display(Name = "TipoOrigemSimulacao_House", ResourceType = typeof(GlobalMessages))]
        House = 2,
        [Display(Name = "TipoOrigemSimulacao_Admin", ResourceType = typeof(GlobalMessages))]
        Admin = 2

    }
}
