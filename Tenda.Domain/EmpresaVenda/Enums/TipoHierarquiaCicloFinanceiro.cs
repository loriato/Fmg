using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoHierarquiaCicloFinanceiro
    {
        [Display(Name = "TipoHierarquiaCicloFinanceiro_CoordenadorSupervisor", ResourceType = typeof(GlobalMessages))]
        CoordenadorSupervisor = 1,
        [Display(Name = "TipoHierarquiaCicloFinanceiro_CoordenadorViabilizador", ResourceType = typeof(GlobalMessages))]
        CoordenadorViabilizador = 2,
        [Display(Name = "TipoHierarquiaCicloFinanceiro_SupervisorViabilizador", ResourceType = typeof(GlobalMessages))]
        SupervisorViabilizador = 3
    }
}
