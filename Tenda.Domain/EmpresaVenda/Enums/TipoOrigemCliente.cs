using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoOrigemCliente
    {
        [Display(Name = "TipoOrigemCliente_Espontaneo", ResourceType = typeof(GlobalMessages))]
        Espontaneo = 1,
        [Display(Name = "TipoOrigemCliente_Lead", ResourceType = typeof(GlobalMessages))]
        Lead = 2,
        [Display(Name = "TipoOrigemCliente_Indicacao", ResourceType = typeof(GlobalMessages))]
        Indicacao = 3,
        [Display(Name = "TipoOrigemCliente_CarteiraEV", ResourceType = typeof(GlobalMessages))]
        CarteiraEV = 4,
        [Display(Name = "TipoOrigemCliente_FeiraoTenda", ResourceType = typeof(GlobalMessages))]
        FeiraoTenda =5,
        [Display(Name = "TipoOrigemCliente_Funcionario", ResourceType = typeof(GlobalMessages))]
        Funcionario =6,
        [Display(Name = "TipoOrigemCliente_Loja", ResourceType = typeof(GlobalMessages))]
        Loja =7,
        [Display(Name = "TipoOrigemCliente_Simulador", ResourceType = typeof(GlobalMessages))]
        Simulador = 8,
    }
}
