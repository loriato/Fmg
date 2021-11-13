using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoRateioComissao
    {
        [Display(Name = "SituacaoRateioComissao_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoRateioComissao_Finalizado", ResourceType = typeof(GlobalMessages))]
        Finalizado = 2,
        [Display(Name = "SituacaoRateioComissao_Rascunho", ResourceType = typeof(GlobalMessages))]
        Rascunho = 3

    }
}
