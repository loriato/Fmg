using Europa.Resources;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    public enum TipoDesistencia
    {
        [Display(Name = "TipoDesistencia_NumeroErradoClienteNaoAtende", ResourceType = typeof(GlobalMessages))]
        NumeroErradoClienteNaoAtende = 6,
        [Display(Name = "TipoDesistencia_Desempregado", ResourceType = typeof(GlobalMessages))]
        Desempregado = 1,
        [Display(Name = "TipoDesistencia_JaPossuiImovel", ResourceType = typeof(GlobalMessages))]
        JaPossuiImovel = 2,
        [Display(Name = "TipoDesistencia_SemInteresse", ResourceType = typeof(GlobalMessages))]
        SemInteresse = 5,
        [Display(Name = "TipoDesistencia_ComRestricao", ResourceType = typeof(GlobalMessages))]
        ComRestricao = 4,
        [Display(Name = "TipoDesistencia_JaComprouComLoja", ResourceType = typeof(GlobalMessages))]
        JaComprouComLoja = 7,
        [Display(Name = "TipoDesistencia_Outros", ResourceType = typeof(GlobalMessages))]
        Outros = 3,
    }
}
