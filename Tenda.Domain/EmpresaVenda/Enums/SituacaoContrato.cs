using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoContrato
    {
        [Display(Name = "SituacaoContrato_Repassado", ResourceType = typeof(GlobalMessages))]
        Repassado = 1,
        [Display(Name = "SituacaoContrato_Conforme", ResourceType = typeof(GlobalMessages))]
        Conforme = 2
    }
}
