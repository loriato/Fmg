using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Commons
{
    public class SessionControllerFactory : DefaultControllerFactory
    {
        protected override SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return SessionStateBehavior.Default;

            var actionName = requestContext.RouteData.Values["action"].ToString();
            Type typeOfRequest = requestContext.HttpContext.Request.RequestType.ToLower() == "get" ? typeof(HttpGetAttribute) : typeof(HttpPostAttribute);
            // [Line1]
            var cntMethods = controllerType.GetMethods()
                   .Where(m =>
                    m.Name == actionName &&
                    ((typeOfRequest == typeof(HttpPostAttribute) &&
                          m.CustomAttributes.Where(a => a.AttributeType == typeOfRequest).Count() > 0
                       )
                       ||
                       (typeOfRequest == typeof(HttpGetAttribute) &&
                          m.CustomAttributes.Where(a => a.AttributeType == typeof(HttpPostAttribute)).Count() == 0
                       )
                    )
                );
            MethodInfo actionMethodInfo = actionMethodInfo = cntMethods != null && cntMethods.Count() == 1 ? cntMethods.ElementAt(0) : null;
            if (actionMethodInfo != null)
            {
                var sessionStateAttr = actionMethodInfo.GetCustomAttributes(typeof(ActionSessionStateAttribute), false)
                                    .OfType<ActionSessionStateAttribute>()
                                    .FirstOrDefault();

                if (sessionStateAttr != null)
                {
                    return sessionStateAttr.SessionBehavior;
                }
            }
            return base.GetControllerSessionBehavior(requestContext, controllerType);
        }
    }
}