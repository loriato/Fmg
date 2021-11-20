using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Europa.Web.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Shared;

namespace Europa.Fmg.Portal.Models.Application
{
    public class SessionAttributes : ISessionAttributes
    {
        private static readonly string SessionName = "CurrentHttpSession";

        public UsuarioPortal UsuarioPortal { get; set; }
        //public Acesso Acesso { get; set; }
        // FIXME: Jogar para Static de Aplicação

        public string LastUser { get; set; }



        public SessionAttributes()
        {
            UsuarioPortal = null;
        }

        public bool IsValidSession(string identity)
        {
            if (UsuarioPortal == null)
            {
                return false;
            }
            return identity != null && identity.Equals(LastUser);
        }

        public bool IsValidSession()
        {
            var user = HttpContext.Current.User;
            if (user == null)
            {
                return false;
            }
            return IsValidSession(user.Identity.Name);
        }

        public void LoginWithUser(UsuarioPortal usuario)
        {
            UsuarioPortal = usuario;
            LastUser = usuario.Login;
        }

        public void Invalidate()
        {
            UsuarioPortal = null;
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static SessionAttributes Current()
        {
            SessionAttributes currentSession = (SessionAttributes)HttpContext.Current.Session[SessionName];
            if (currentSession == null)
            {
                currentSession = new SessionAttributes();
                HttpContext.Current.Session[SessionName] = currentSession;
            }
            return currentSession;
        }

        public long GetUser()
        {
            if (Current() != null && Current().UsuarioPortal != null)
            {
                return Current().UsuarioPortal.Id;
            }
            return 0;
        }

        //public long GetAccess()
        //{
        //    if (Current() != null && Current().UsuarioPortal != null)
        //    {
        //        return Current().Acesso.Id;
        //    }
        //    return 0;
        //}


    }
}