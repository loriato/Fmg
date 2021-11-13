using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoFuncao
    {
        [Display(Name = "TipoFuncao_Corretor", ResourceType = typeof(GlobalMessages))]
        Corretor = 0,
        [Display(Name = "TipoFuncao_Coordenador", ResourceType = typeof(GlobalMessages))]
        Coordenador = 1,
        [Display(Name = "TipoFuncao_Gerente", ResourceType = typeof(GlobalMessages))]
        Gerente = 2,
        [Display(Name = "TipoFuncao_Diretor", ResourceType = typeof(GlobalMessages))]
        Diretor = 3
    }
}