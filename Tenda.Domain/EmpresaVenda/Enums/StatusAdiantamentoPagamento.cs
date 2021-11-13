using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    public enum StatusAdiantamentoPagamento
    {
        [TypeConverter(typeof(LocalizedEnumConverter))]
        [Display(Name = "StatusAdiantamentoPagamento_Aprovado", ResourceType = typeof(GlobalMessages))]
        Aprovado = 1,
        [Display(Name = "StatusAdiantamentoPagamento_Reprovado", ResourceType = typeof(GlobalMessages))]
        Reprovado = 2,
        [Display(Name = "StatusAdiantamentoPagamento_Solicitado", ResourceType = typeof(GlobalMessages))]
        Solicitado = 3
    }
}
