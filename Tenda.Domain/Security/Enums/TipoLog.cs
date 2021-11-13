using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Security.Enums
{

    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoLog
    {
        [Display(Name = "TipoLog_NaoDefinido", ResourceType = typeof(GlobalMessages))]
        NaoDefinido = 0,
        [Display(Name = "TipoLog_Informacao", ResourceType = typeof(GlobalMessages))]
        Informacao = 1,
        [Display(Name = "TipoLog_Aviso", ResourceType = typeof(GlobalMessages))]
        Aviso = 2,
        [Display(Name = "TipoLog_Erro", ResourceType = typeof(GlobalMessages))]
        Erro = 3
    }
}
