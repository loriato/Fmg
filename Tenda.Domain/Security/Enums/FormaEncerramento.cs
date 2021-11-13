using System.ComponentModel;
using Europa.Resources;

namespace Tenda.Domain.Security.Enums
{
    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum FormaEncerramento
    {
        Forced = 1,
        Logout = 2,
        TimeOut = 3,
        Unknown = 4
    }
}
