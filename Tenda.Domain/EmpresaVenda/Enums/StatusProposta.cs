using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum StatusProposta
    {
        [Display(Name = "StatusProposta_EmElaboracao", ResourceType = typeof(GlobalMessages))]
        EmElaboracao = 1,
        [Display(Name = "StatusProposta_Concluida", ResourceType = typeof(GlobalMessages))]
        Concluida = 2,
        [Display(Name = "StatusProposta_Cancelada", ResourceType = typeof(GlobalMessages))]
        Cancelada = 3,
        [Display(Name = "StatusProposta_Liberada", ResourceType = typeof(GlobalMessages))]
        Liberada = 4,
        [Display(Name = "StatusProposta_Revisao", ResourceType = typeof(GlobalMessages))]
        Revisao = 5
    }
}
