using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoReceitaFederal
    {
        [Display(Name = "TipoReceitaFederal_Ok", ResourceType = typeof(GlobalMessages))]
        Ok = 1,
        [Display(Name = "TipoReceitaFederal_Indisponivel", ResourceType = typeof(GlobalMessages))]
        Indisponivel = 2,
        [Display(Name = "TipoReceitaFederal_Restricao", ResourceType = typeof(GlobalMessages))]
        Restricao = 3,
        [Display(Name = "TipoReceitaFederal_Pendente", ResourceType = typeof(GlobalMessages))]
        Pendente = 4
    }
}
