using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoSimNao
    {
        [Display(Name = "TipoSimNao_Sim", ResourceType = typeof(GlobalMessages))]
        Sim = 1,
        [Display(Name = "TipoSimNao_Nao", ResourceType = typeof(GlobalMessages))]
        Nao = 2
    }
}
