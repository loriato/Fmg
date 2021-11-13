using Europa.Extensions;
using System.Collections.Generic;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Core.Data
{
    public class RequestStateApi
    {
        public UsuarioPortal UsuarioPortal { get; set; }
        public Acesso Acesso { get; set; }
        public List<Perfil> Perfis { get; set; }
        public string UnidadeFuncional { get; set; }
        public string Funcionalidade { get; set; }
        public string CodigoSistema { get; set; }

        public bool IsValidSession(string identity)
        {
            if (UsuarioPortal == null) return false;
            return identity != null;
        }

        public void LoginWithUser(UsuarioPortal usuario, Acesso acesso, List<Perfil> perfis)
        {
            UsuarioPortal = usuario;
            Perfis = perfis;
            Acesso = acesso;
            CodigoSistema = acesso.Sistema.Codigo;
        }

        public void Invalidate()
        {
            UsuarioPortal = null;
            Perfis = null;
            Acesso = null;
            CodigoSistema = null;
        }

        public long GetUserPrimaryKey()
        {
            return UsuarioPortal != null ? UsuarioPortal.Id : 0;
        }

        public long GetAccessPrimaryKey()
        {
            return Acesso != null ? Acesso.Id : 0;
        }

        public string GetSystemCode()
        {
            return CodigoSistema.HasValue() ? CodigoSistema : "";
        }
    }
}
