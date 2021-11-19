using Autofac;
using Autofac.Integration.Mvc;
using Europa.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Core.Data
{
    public class RequestState
    {
        public long IdUsuario { get; set; }
        public Acesso Acesso { get; set; }
        public long IdAcesso { get; set; }
        public string UnidadeFuncional { get; set; }
        public string Funcionalidade { get; set; }
        public UsuarioPortal UsuarioPortal { get; set; }

        public RequestState()
        {
            var session = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<ISessionAttributes>();
            IdUsuario = session.GetUser();
            //IdAcesso = session.GetAccess();
        }

        public bool IsValidSession(string identity)
        {
            if (UsuarioPortal == null) return false;
            return identity != null;
        }

        public void LoginWithUser(UsuarioPortal usuario, Acesso acesso)
        {
            UsuarioPortal = usuario;
            Acesso = acesso;
        }

        public void Invalidate()
        {
            UsuarioPortal = null;

            Acesso = null;
        }

        public long GetUserPrimaryKey()
        {
            return UsuarioPortal != null ? UsuarioPortal.Id : 0;
        }

        public long GetAccessPrimaryKey()
        {
            return Acesso != null ? Acesso.Id : 0;
        }
    }
}
