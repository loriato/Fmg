using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoSexo
    {
        [Display(Name = "TipoSexo_Feminino", ResourceType = typeof(GlobalMessages))]
        Feminino = 1,
        [Display(Name = "TipoSexo_Masculino", ResourceType = typeof(GlobalMessages))]
        Masculino = 2,
        [Display(Name = "TipoSexo_Outros", ResourceType = typeof(GlobalMessages))]
        Outros = 3
    }
}
