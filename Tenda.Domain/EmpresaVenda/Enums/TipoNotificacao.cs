using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoNotificacao
    {
        [Display(Name = "TipoNotificacao_Comum", ResourceType = typeof(GlobalMessages))]
        Comum = 1,
        [Display(Name = "TipoNotificacao_Lead", ResourceType = typeof(GlobalMessages))]
        Lead = 2
    }
}
