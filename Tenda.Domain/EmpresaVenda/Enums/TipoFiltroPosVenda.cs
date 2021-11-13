using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoFiltroPosVenda
    {
        [Display(Name = "TipoFiltroPosVenda_SemDocsAvalista", ResourceType = typeof(GlobalMessages))]
        SemDocsAvalista = 1,
        [Display(Name = "TipoFiltroPosVenda_DocsAvalistaEnviados", ResourceType = typeof(GlobalMessages))]
        DocsAvalistaEnviados = 2,
        [Display(Name = "TipoFiltroPosVenda_AvalistaPreAprovado", ResourceType = typeof(GlobalMessages))]
        AvalistaPreAprovado = 3
    }
}
