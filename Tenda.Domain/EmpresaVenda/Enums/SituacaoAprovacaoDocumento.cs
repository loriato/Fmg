using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoAprovacaoDocumento
    {
        [Display(Name = "SituacaoAprovacaoDocumento_NaoAnexado", ResourceType = typeof(GlobalMessages))]
        NaoAnexado = 1,
        [Display(Name = "SituacaoAprovacaoDocumento_Anexado", ResourceType = typeof(GlobalMessages))]
        Anexado = 2,
        [Display(Name = "SituacaoAprovacaoDocumento_Aprovado", ResourceType = typeof(GlobalMessages))]
        Aprovado = 3,
        [Display(Name = "SituacaoAprovacaoDocumento_Pendente", ResourceType = typeof(GlobalMessages))]
        Pendente = 4,
        [Display(Name = "SituacaoAprovacaoDocumento_Informado", ResourceType = typeof(GlobalMessages))]
        Informado = 5,
        [Display(Name = "SituacaoAprovacaoDocumento_Enviado", ResourceType = typeof(GlobalMessages))]
        Enviado = 6,
        [Display(Name = "SituacaoAprovacaoDocumento_PreCarregado", ResourceType = typeof(GlobalMessages))]
        PreCarregado = 7,
    }
}