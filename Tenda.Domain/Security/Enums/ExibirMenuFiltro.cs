using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Europa.Resources;

namespace Tenda.Domain.Security.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum ExibirMenuFiltro
    {
        [Display(Name = "ExibirMenu_Todos", ResourceType = typeof(GlobalMessages))]
        Todos = 0,
        [Display(Name = "ExibirMenu_Sim", ResourceType = typeof(GlobalMessages))]
        Sim = 1,
        [Display(Name = "ExibirMenu_Nao", ResourceType = typeof(GlobalMessages))]
        Nao = 2

    }
}