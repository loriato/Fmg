using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.EmpresaVenda.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoParcela
    {
        [Display(Name = "TipoParcela_Ano", ResourceType = typeof(GlobalMessages))]
        Ano = 1,
        [Display(Name = "TipoParcela_Mensal", ResourceType = typeof(GlobalMessages))]
        Mensal = 2,
        [Display(Name = "TipoParcela_Repasse", ResourceType = typeof(GlobalMessages))]
        Repasse = 3,
        [Display(Name = "TipoParcela_Ato", ResourceType = typeof(GlobalMessages))]
        Ato = 4,
        [Display(Name = "TipoParcela_Semestral", ResourceType = typeof(GlobalMessages))]
        Semestral = 5,
        [Display(Name = "TipoParcela_Bimestral", ResourceType = typeof(GlobalMessages))]
        Bimestral = 6,
        [Display(Name = "TipoParcela_PreChaves", ResourceType = typeof(GlobalMessages))]
        PreChaves = 8,
        [Display(Name = "TipoParcela_PreChavesIntermediaria", ResourceType = typeof(GlobalMessages))]
        PreChavesIntermediaria = 9,
        [Display(Name = "TipoParcela_PosChaves", ResourceType = typeof(GlobalMessages))]
        PosChaves = 10,
        [Display(Name = "TipoParcela_Financiamento", ResourceType = typeof(GlobalMessages))]
        Financiamento = 11,
        [Display(Name = "TipoParcela_Subsidio", ResourceType = typeof(GlobalMessages))]
        Subsidio = 12,
        [Display(Name = "TipoParcela_FGTS", ResourceType = typeof(GlobalMessages))]
        FGTS = 13,
        [Display(Name = "TipoParcela_PreChavesItbi", ResourceType = typeof(GlobalMessages))]
        PreChavesItbi = 14,
        [Display(Name = "TipoParcela_PreChavesIntermediariaItbi", ResourceType = typeof(GlobalMessages))]
        PreChavesIntermediariaItbi = 15,
        [Display(Name = "TipoParcela_PosChavesItbi", ResourceType = typeof(GlobalMessages))]
        PosChavesItbi = 16,
        [Display(Name = "TipoParcela_PremiadaTenda", ResourceType = typeof(GlobalMessages))]
        PremiadaTenda = 17
    }
}
