using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum StatusSicaq
    {
        [Display(Name = "StatusSicaq_Aprovado", ResourceType = typeof(GlobalMessages))]
        Aprovado = 1,
        [Display(Name = "StatusSicaq_Condicionado", ResourceType = typeof(GlobalMessages))]
        Condicionado = 2,
        [Display(Name = "StatusSicaq_EmProcessamento", ResourceType = typeof(GlobalMessages))]
        EmProcessamento = 3,
        [Display(Name = "StatusSicaq_Erro", ResourceType = typeof(GlobalMessages))]
        Erro = 4,
        [Display(Name = "StatusSicaq_Reprovado", ResourceType = typeof(GlobalMessages))]
        Reprovado = 5,
        [Display(Name = "StatusSicaq_ComErro", ResourceType = typeof(GlobalMessages))]
        SICAQComErro = 6,
        [Display(Name = "StatusSicaq_Previo", ResourceType = typeof(GlobalMessages))]
        Previo = 7
    }
}
