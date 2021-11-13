using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoPontuacaoFidelidade
    {
        [Display(Name = "SituacaoPontuacaoFidelidade_Rascunho", ResourceType = typeof(GlobalMessages))]
        Rascunho = 0,
        [Display(Name = "SituacaoPontuacaoFidelidade_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoPontuacaoFidelidade_Vencido", ResourceType = typeof(GlobalMessages))]
        Vencido = 2,
        [Display(Name = "SituacaoPontuacaoFidelidade_Suspenso", ResourceType = typeof(GlobalMessages))]
        Suspenso = 3,
        [Display(Name = "SituacaoPontuacaoFidelidade_AguardandoLiberacao", ResourceType = typeof(GlobalMessages))]
        AguardandoLiberacao = 4
    }
}
