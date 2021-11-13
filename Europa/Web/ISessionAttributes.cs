using System.Collections.Generic;

namespace Europa.Web
{
    public interface ISessionAttributes
    {
        long GetUser();
        long GetAccess();
        List<long> GetRoles();
        bool HasPermission(string codigoUnidadeFuncional, string comandoFuncionalidade);
    }
}
