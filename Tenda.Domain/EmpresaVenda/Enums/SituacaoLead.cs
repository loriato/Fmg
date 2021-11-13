using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoLead
    {
        [Display(Name = "SituacaoLead_Contato", ResourceType = typeof(GlobalMessages))]
        Contato = 1,
        [Display(Name = "SituacaoLead_ContatoNaoAtende", ResourceType = typeof(GlobalMessages))]
        ContatoNaoAtende = 6,
        [Display(Name = "SituacaoLead_ContatoBemSucedido", ResourceType = typeof(GlobalMessages))]
        ContatoBemSucedido = 7,
        [Display(Name = "SituacaoLead_AgendamentoVisita", ResourceType = typeof(GlobalMessages))]
        AgendamentoVisita = 4,
        [Display(Name = "SituacaoLead_AgendamentoEnvioDocOnline", ResourceType = typeof(GlobalMessages))]
        AgendamentoEnvioDocOnline = 5,
        [Display(Name = "SituacaoLead_PPRGeradaSicaqAprovado", ResourceType = typeof(GlobalMessages))]
        PPRGeradaSicaqAprovado = 8,
        [Display(Name = "SituacaoLead_PPRGeradaSicaqCondicionado", ResourceType = typeof(GlobalMessages))]
        PPRGeradaSicaqCondicionado = 9,
        [Display(Name = "SituacaoLead_PPRGeradaSicaqReprovado", ResourceType = typeof(GlobalMessages))]
        PPRGeradaSicaqReprovado = 10,
        [Display(Name = "SituacaoLead_PPRGeradaVendaGerada", ResourceType = typeof(GlobalMessages))]
        PPRGeradaVendaGerada = 3,
        [Display(Name = "SituacaoLead_Desistencia", ResourceType = typeof(GlobalMessages))]
        Desistencia = 2,
    }
}






