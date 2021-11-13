using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum DestinoNotificacao
    {
        [Display(Name = "DestinoNotificacao_Todos", ResourceType = typeof(GlobalMessages))]
        Todos = 0,
        [Display(Name = "DestinoNotificacao_Portal", ResourceType = typeof(GlobalMessages))]
        Portal = 1,
        [Display(Name = "DestinoNotificacao_Adm", ResourceType = typeof(GlobalMessages))]
        Adm = 2
    }
}
