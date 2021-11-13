using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoResgate
    {
        [Display(Name = "SituacaoResgate_Solicitado", ResourceType = typeof(GlobalMessages))]
        Solicitado = 0,
        [Display(Name = "SituacaoResgate_Liberado", ResourceType = typeof(GlobalMessages))]
        Liberado = 1
        //[Display(Name = "SituacaoResgate_Reprovado", ResourceType = typeof(GlobalMessages))]
        //Reprovado = 2
    }
}
