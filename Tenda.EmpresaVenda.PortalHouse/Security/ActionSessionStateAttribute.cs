using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Tenda.EmpresaVenda.PortalHouse.Security
{
    public sealed class ActionSessionStateAttribute : Attribute
    {
        public ActionSessionStateAttribute(SessionStateBehavior sessionBehavior)
        {
            SessionBehavior = sessionBehavior;
        }

        public SessionStateBehavior SessionBehavior { get; }
    }
}