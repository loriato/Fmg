using System.ComponentModel;
using Europa.Resources;

namespace Tenda.Domain.Security.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum Situacao
    {
        Ativo = 1,
        Suspenso = 2, 
        Cancelado = 3,
        PreCadastro = 4
    }
}