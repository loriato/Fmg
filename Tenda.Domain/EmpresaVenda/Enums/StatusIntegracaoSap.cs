using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum StatusIntegracaoSap
    {
        [Display(Name = "StatusIntegracaoSap_AguardandoRC", ResourceType = typeof(GlobalMessages))]
        AguardandoRC = 0,
        [Display(Name = "StatusIntegracaoSap_AguardandoPedido", ResourceType = typeof(GlobalMessages))]
        AguardandoPedido = 1,
        [Display(Name = "StatusIntegracaoSap_PedidoGerado", ResourceType = typeof(GlobalMessages))]
        PedidoGerado = 2

    }
}
