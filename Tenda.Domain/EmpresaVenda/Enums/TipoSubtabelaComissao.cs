using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoSubtabelaComissao
    {
        [Display(Name = "TipoSubtabelaComissao_Seca", ResourceType = typeof(GlobalMessages))]
        Seca = 1,
        [Display(Name = "TipoSubtabelaComissao_Normal", ResourceType = typeof(GlobalMessages))]
        Normal =2,
        [Display(Name = "TipoSubtabelaComissao_Turbinada", ResourceType = typeof(GlobalMessages))]
        Turbinada =3
    }
}
