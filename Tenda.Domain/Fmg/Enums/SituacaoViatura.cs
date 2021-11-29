using Europa.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tenda.Domain.Fmg.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum SituacaoViatura
    {
        [Display(Name = "SituacaoViatura_Ativo", ResourceType = typeof(GlobalMessages))]
        Ativo = 1,
        [Display(Name = "SituacaoViatura_EmPercurso", ResourceType = typeof(GlobalMessages))]
        EmPercurso = 2,
        [Display(Name = "SituacaoViatura_EmManutencao", ResourceType = typeof(GlobalMessages))]
        EmManutencao = 3
    }
}
