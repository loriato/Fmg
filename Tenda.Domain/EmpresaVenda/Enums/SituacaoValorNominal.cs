using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoValorNominal
    {
        [Display(Name = "SituacaoValorNominal_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoValorNominal_Vencido", ResourceType = typeof(GlobalMessages))]
        Vencido = 2
    }
}
