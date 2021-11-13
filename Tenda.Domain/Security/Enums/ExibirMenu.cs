using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Security.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum ExibirMenu
    {
        [Display(Name = "ExibirMenu_Nao", ResourceType = typeof(GlobalMessages))]
        Nao = 0,
        [Display(Name = "ExibirMenu_Sim", ResourceType = typeof(GlobalMessages))]
        Sim = 1
    }
}