using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoAvalista
    {
        [Display(Name = "SituacaoAvalista_DocumentosPendentes", ResourceType = typeof(GlobalMessages))]
        DocumentosPendentes = 1,
        [Display(Name = "SituacaoAvalista_DocumentosAnexados", ResourceType = typeof(GlobalMessages))]
        DocumentosAnexados = 2,
        [Display(Name = "SituacaoAvalista_AvalistaAprovado", ResourceType = typeof(GlobalMessages))]
        AvalistaAprovado = 3,
        [Display(Name = "SituacaoAvalista_AvalistaReprovado", ResourceType = typeof(GlobalMessages))]
        AvalistaReprovado = 4,
        [Display(Name = "SituacaoAvalista_AguardandoAnalise", ResourceType = typeof(GlobalMessages))]
        AguardandoAnalise = 5,
    }
}