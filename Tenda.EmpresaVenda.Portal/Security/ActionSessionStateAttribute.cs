using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Europa.Fmg.Portal.Security
{
    public sealed class ActionSessionStateAttribute : Attribute
    {
        public SessionStateBehavior SessionBehavior { get; private set; }

        public ActionSessionStateAttribute(SessionStateBehavior sessionBehavior)
        {
            SessionBehavior = sessionBehavior;
        }
    }
}