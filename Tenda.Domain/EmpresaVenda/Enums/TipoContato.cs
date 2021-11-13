using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoContato
    {
        [Display(Name = "TipoContato_Email", ResourceType = typeof(GlobalMessages))]
        Email = 2,
        [Display(Name = "TipoContato_Celular", ResourceType = typeof(GlobalMessages))]
        Celular = 3,
        [Display(Name = "TipoContato_Residencial", ResourceType = typeof(GlobalMessages))]
        Residencial = 4,
        [Display(Name = "TipoContato_Comercial", ResourceType = typeof(GlobalMessages))]
        Comercial = 5
    }
}
