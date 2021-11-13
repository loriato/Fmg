//Referência https://dotcms.com/docs/latest/active-directory-error-codes

namespace Europa.Commons.LDAP
{
    public static class LdapError
    {
        public static string UserNotFound = "525";
        public static string InvalidCredentials = "52e";
        public static string NotPermittedToLogon = "530";
        public static string PasswordExpired = "532";
        public static string AccountDisabled = "533";
        public static string AccountExpired = "701";
        public static string UserMustResetPassword = "773";
        public static string AccountLocked = "775";
    }
}