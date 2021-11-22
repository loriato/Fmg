using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoPontuacaoFidelidade
    {
        [Display(Name = "TipoPontuacaoFidelidade_Normal", ResourceType = typeof(GlobalMessages))]
        Normal = 1,
        [Display(Name = "TipoPontuacaoFidelidade_Campanha", ResourceType = typeof(GlobalMessages))]
        Campanha = 2
    }
}
