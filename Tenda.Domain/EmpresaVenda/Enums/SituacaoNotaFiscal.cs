using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoNotaFiscal
    {
        [Display(Name = "SituacaoNotaFiscal_PendenteEnvio", ResourceType = typeof(GlobalMessages))]
        PendenteEnvio = 0,
        [Display(Name = "SituacaoNotaFiscal_Aprovado", ResourceType = typeof(GlobalMessages))]
        Aprovado = 1,
        [Display(Name = "SituacaoNotaFiscal_AguardandoProcessamento", ResourceType = typeof(GlobalMessages))]
        AguardandoProcessamento = 2,
        [Display(Name = "SituacaoNotaFiscal_Reprovado", ResourceType = typeof(GlobalMessages))]
        Reprovado = 3,
        [Display(Name = "SituacaoNotaFiscal_AguardandoAvaliacao", ResourceType = typeof(GlobalMessages))]
        AguardandoAvaliacao = 4,
        [Display(Name = "SituacaoNotaFiscal_Distratado", ResourceType = typeof(GlobalMessages))]
        Distratado = 5,
        [Display(Name = "SituacaoNotaFiscal_PreAprovado", ResourceType = typeof(GlobalMessages))]
        PreAprovado = 6,
        [Display(Name = "SituacaoNotaFiscal_AguardandoEnvioMidas", ResourceType = typeof(GlobalMessages))]
        AguardandoEnvioMidas = 7
    }
}
