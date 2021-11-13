using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoAprovacaoDocumentoAvalista
    {
        [Display(Name = "SituacaoAprovacaoDocumentoAvalista_NaoAnexado", ResourceType = typeof(GlobalMessages))]
        NaoAnexado = 1,
        [Display(Name = "SituacaoAprovacaoDocumentoAvalista_Anexado", ResourceType = typeof(GlobalMessages))]
        Anexado = 2,
        [Display(Name = "SituacaoAprovacaoDocumentoAvalista_PreAprovado", ResourceType = typeof(GlobalMessages))]
        PreAprovado = 3,
        [Display(Name = "SituacaoAprovacaoDocumentoAvalista_Pendente", ResourceType = typeof(GlobalMessages))]
        Pendente = 4,
        [Display(Name = "SituacaoAprovacaoDocumentoAvalista_Informado", ResourceType = typeof(GlobalMessages))]
        Informado = 5,
        [Display(Name = "SituacaoAprovacaoDocumentoAvalista_Enviado", ResourceType = typeof(GlobalMessages))]
        Enviado = 6,
        [Display(Name = "SituacaoAprovacaoDocumentoAvalista_PreCarregado", ResourceType = typeof(GlobalMessages))]
        PreCarregado = 7,
    }
}