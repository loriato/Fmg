using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoRegraComissao
    {
        [Display(Name = "SituacaoRegraComissao_Rascunho", ResourceType = typeof(GlobalMessages))]
        Rascunho = 0,
        [Display(Name = "SituacaoRegraComissao_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoRegraComissao_Vencido", ResourceType = typeof(GlobalMessages))]
        Vencido = 2,
        [Display(Name = "SituacaoRegraComissao_SuspensoPorCampanha", ResourceType =typeof(GlobalMessages))]
        SuspensoPorCampanha = 3,
        [Display(Name = "SituacaoRegraComissao_AguardandoLiberacao", ResourceType = typeof(GlobalMessages))]
        AguardandoLiberacao = 4
    }
}
