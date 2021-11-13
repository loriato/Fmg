using System.Collections.Generic;
using System.Linq;
using System.Web;
using Europa.Extensions;
using Europa.Web;
using Tenda.EmpresaVenda.ApiService.Models.Login;

namespace Tenda.EmpresaVenda.PortalHouse.Models.Application
{
    public class SessionAttributes : ISessionAttributesApi
    {
        private static readonly string SESSION_NAME = "CurrentHttpSession";

        public SessionAttributes()
        {
            UsuarioPortal = null;
        }

        private string LastUser { get; set; }

        public LoginResponseDto UsuarioPortal { get; set; }

        public string GetUserToken()
        {
            if (Current() != null && Current().UsuarioPortal != null) return Current().UsuarioPortal.Autorizacao;
            return null;
        }

        public bool HasPermission(string codigoUnidadeFuncional, string comandoFuncionalidade)
        {
            if (codigoUnidadeFuncional.IsEmpty() || UsuarioPortal.Permissoes.IsEmpty()) return false;
            List<string> comandos;
            if (UsuarioPortal.Permissoes.Any(x => x.UnidadeFuncional.Equals(codigoUnidadeFuncional)))
            {
                comandos = UsuarioPortal.Permissoes.Where(x => x.UnidadeFuncional.Equals(codigoUnidadeFuncional))
                    .Select(x => x.Funcionalidades).FirstOrDefault();
                if (comandoFuncionalidade.IsNull()) return true;
                return comandos.Contains(comandoFuncionalidade);
            }

            return false;
        }

        public long GetUserPrimaryKey()
        {
            if (Current() != null && Current().UsuarioPortal != null) return Current().UsuarioPortal.Id;
            return 0;
        }

        public long GetAccessPrimaryKey()
        {
            if (Current() != null && Current().UsuarioPortal != null) return Current().UsuarioPortal.IdAcesso;
            return 0;
        }

        public bool IsValidSession(string identity)
        {
            if (UsuarioPortal == null) return false;
            return identity != null && identity.Equals(LastUser);
        }

        public bool IsValidSession()
        {
            var user = HttpContext.Current.User;
            if (user == null) return false;
            return IsValidSession(user.Identity.Name);
        }

        public void Invalidate()
        {
            UsuarioPortal = null;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public void LoginWithUser(LoginResponseDto responseDto)
        {
            UsuarioPortal = responseDto;
            LastUser = responseDto.Login;
        }

        public static SessionAttributes Current()
        {
            var currentSession = (SessionAttributes) HttpContext.Current.Session[SESSION_NAME];
            if (currentSession == null)
            {
                currentSession = new SessionAttributes();
                HttpContext.Current.Session[SESSION_NAME] = currentSession;
            }

            return currentSession;
        }

        public long GetUser()
        {
            throw new System.NotImplementedException();
        }
    }
}