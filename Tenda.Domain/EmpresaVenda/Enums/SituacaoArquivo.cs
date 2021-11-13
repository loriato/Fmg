using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoArquivo
    {
        [Display(Name = "SituacaoArquivo_AguardandoProcessamento", ResourceType = typeof(GlobalMessages))]
        AguardandoProcessamento = 1,
        [Display(Name = "SituacaoArquivo_EmProcessamento", ResourceType = typeof(GlobalMessages))]
        EmProcessamento = 2,
        [Display(Name = "SituacaoArquivo_Erro", ResourceType = typeof(GlobalMessages))]
        Erro = 3,
        [Display(Name = "SituacaoArquivo_Processado", ResourceType = typeof(GlobalMessages))]
        Processado = 4,
        [Display(Name = "SituacaoArquivo_ProcessadoRessalva", ResourceType = typeof(GlobalMessages))]
        ProcessadoRessalva = 5
    }
}
