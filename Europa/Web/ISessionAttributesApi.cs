namespace Europa.Web
{
    public interface ISessionAttributesApi
    {
        long GetUser();
        bool HasPermission(string codigoUnidadeFuncional, string comandoFuncionalidade);
        long GetUserPrimaryKey();

        long GetAccessPrimaryKey();

        string GetUserToken();
    }
}
