using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoPagamento
    {
        [Display(Name = "TipoPagamento_KitCompleto", ResourceType = typeof(GlobalMessages))]
        KitCompleto = 1,
        [Display(Name = "TipoPagamento_Repasse", ResourceType = typeof(GlobalMessages))]
        Repasse = 2,
        [Display(Name = "TipoPagamento_Conformidade", ResourceType = typeof(GlobalMessages))]
        Conformidade = 3        
    }
}
