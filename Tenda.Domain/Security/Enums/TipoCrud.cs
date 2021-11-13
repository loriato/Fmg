using System.ComponentModel;
using Europa.Resources;

namespace Tenda.Domain.Security.Enums
{

    [TypeConverter(typeof(LocalizedEnumConverter))]
    public enum TipoCrud
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4
    }

}
