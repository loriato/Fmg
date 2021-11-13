using System;

namespace Europa.Commons.LDAP
{
    public class LdapException : Exception
    {
        public LdapException(string message, string errorCode, string extendedErrorMessage) : base(message)
        {
            ErrorCode = errorCode;
            ExtendedErrorMessage = extendedErrorMessage;
        }

        public string ErrorCode { get; set; }
        public string ExtendedErrorMessage { get; set; }
    }
}
