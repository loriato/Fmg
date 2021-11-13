using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoProposta
    {
        [Display(Name = "SituacaoProposta_EmElaboracao", ResourceType = typeof(GlobalMessages))]
        EmElaboracao = 1,
        [Display(Name = "SituacaoProposta_AnaliseSimplificadaAprovada", ResourceType = typeof(GlobalMessages))]
        AnaliseSimplificadaAprovada = 2,
        [Display(Name = "SituacaoProposta_Reprovada", ResourceType = typeof(GlobalMessages))]
        Reprovada = 3,
        [Display(Name = "SituacaoProposta_Cancelada", ResourceType = typeof(GlobalMessages))]
        Cancelada = 4,
        [Display(Name = "SituacaoProposta_Enviada", ResourceType = typeof(GlobalMessages))]
        Enviada = 5,
        [Display(Name = "SituacaoProposta_AguardandoAnaliseSimplificada", ResourceType = typeof(GlobalMessages))]
        AguardandoAnaliseSimplificada = 6,
        [Display(Name = "SituacaoProposta_EmAnaliseSimplificada", ResourceType = typeof(GlobalMessages))]
        EmAnaliseSimplificada = 7,
        [Display(Name = "SituacaoProposta_DocsInsuficientesSimplificado", ResourceType = typeof(GlobalMessages))]
        DocsInsuficientesSimplificado = 8,
        [Display(Name = "SituacaoProposta_Retorno", ResourceType = typeof(GlobalMessages))]
        Retorno = 9,
        [Display(Name = "SituacaoProposta_AguardandoIntegracao", ResourceType = typeof(GlobalMessages))]
        AguardandoIntegracao = 10,
        [Display(Name ="SituacaoProposta_Condicionada", ResourceType =typeof(GlobalMessages))]
        Condicionada = 11,
        [Display(Name ="SituacaoProposta_AguardandoFluxo",ResourceType =typeof(GlobalMessages))]
        AguardandoFluxo = 12,
        [Display(Name ="SituacaoProposta_AguardandoAuditoria",ResourceType =typeof(GlobalMessages))]
        AguardandoAuditoria = 13,
        [Display(Name = "SituacaoProposta_FluxoEnviado", ResourceType =typeof(GlobalMessages))]
        FluxoEnviado = 14,
        [Display(Name = "SituacaoProposta_SICAQComErro", ResourceType =typeof(GlobalMessages))]
        SICAQComErro = 15,
        [Display(Name = "SituacaoProposta_Integrada", ResourceType = typeof(GlobalMessages))]
        Integrada = 16,
        [Display(Name = "SituacaoProposta_AguardandoAnaliseCompleta", ResourceType = typeof(GlobalMessages))]
        AguardandoAnaliseCompleta = 17,
        [Display(Name = "SituacaoProposta_EmAnaliseCompleta", ResourceType = typeof(GlobalMessages))]
        EmAnaliseCompleta = 18,
        [Display(Name = "SituacaoProposta_DocsInsuficientesCompleta", ResourceType = typeof(GlobalMessages))]
        DocsInsuficientesCompleta = 19,
        [Display(Name = "SituacaoProposta_AnaliseCompletaAprovada", ResourceType = typeof(GlobalMessages))]
        AnaliseCompletaAprovada = 20

    }
}