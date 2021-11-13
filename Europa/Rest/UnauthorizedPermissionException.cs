using System;

namespace Europa.Rest
{
    public class UnauthorizedPermissionException : Exception
    {
        public UnauthorizedPermissionException(string message, string unidadeFuncional, string funcionalidade) :
            base(message)
        {
            UnidadeFuncional = unidadeFuncional;
            Funcionalidade = funcionalidade;
        }

        public string UnidadeFuncional { get; }
        public string Funcionalidade { get; }
    }
}