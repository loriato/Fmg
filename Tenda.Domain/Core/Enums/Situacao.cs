using Europa.Resources;
using System.ComponentModel;

namespace Tenda.Domain.Core.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum Situacao
    {
        Ativo = 1,
        Suspenso = 2,
        Cancelado = 3
    }
}