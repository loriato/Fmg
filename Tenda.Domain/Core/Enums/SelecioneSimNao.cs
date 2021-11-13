using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Core.Enums
{

    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SelecioneSimNao
    {
        [Display(Name = "Selecione", ResourceType = typeof(GlobalMessages))]
        Selecione = 0,
        [Display(Name = "Sim", ResourceType = typeof(GlobalMessages))]
        Sim = 1,
        [Display(Name = "Nao", ResourceType = typeof(GlobalMessages))]
        Nao = 2
    }
}
