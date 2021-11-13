using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using Tenda.EmpresaVenda.PortalHouse.Models.Application;

namespace Tenda.EmpresaVenda.PortalHouse.Security
{
    public sealed class ExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var inner = filterContext.Exception;
            while (inner.InnerException != null) inner = inner.InnerException;

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorResponse = new BaseResponse();
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (filterContext.Exception is UnauthorizedAccessException)
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    InvalidateUser();
                    errorResponse.Code = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Messages.Add(GlobalMessages.AcessoInvalido);
                }
                else if (filterContext.Exception is UnauthorizedPermissionException unauthorizedPermissionException)
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse.Code = (int)HttpStatusCode.Forbidden;
                    var message = string.Format(GlobalMessages.MsgAcessoNegado,
                        string.Join(", ", SessionAttributes.Current().UsuarioPortal.Perfis.Select(x => x.Nome)),
                        unauthorizedPermissionException.UnidadeFuncional,
                        unauthorizedPermissionException.Funcionalidade);
                    errorResponse.Messages.Add(message);
                }
                else if (filterContext.Exception is InternalServerException internalServerException)
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Code = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Messages.Add(internalServerException.GetExceptionDto().ExceptionMessage);
                    errorResponse.Data = internalServerException.GetExceptionDto().StackTrace;
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Code = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Messages.Add(inner.Message);
                    errorResponse.Data = inner.StackTrace;
                }

                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = errorResponse
                };
            }
            else
            {
                if (filterContext.Exception is UnauthorizedPermissionException unauthorizedPermissionException)
                {
                    var message = string.Format(GlobalMessages.MsgAcessoNegado,
                        string.Join(", ", SessionAttributes.Current().UsuarioPortal.Perfis.Select(x => x.Nome)),
                        unauthorizedPermissionException.UnidadeFuncional,
                        unauthorizedPermissionException.Funcionalidade);
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new
                        {
                            controller = "Home",
                            action = "AcessoNegado",
                            message
                        }));
                }
                else if (filterContext.Exception is UnauthorizedAccessException)
                {
                    InvalidateUser();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new
                        {
                            controller = "Login",
                            action = "Index",
                        }));
                }
                else if (filterContext.Exception is NotFoundException)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                        new
                        {
                            controller = "Error",
                            action = "NotFound",
                        }));
                }
                else if (filterContext.Exception is InternalServerException internalServerException)
                {
                    var view = new ViewResult { ViewName = "Error" };
                    view.ViewBag.Message = internalServerException.GetExceptionDto().ExceptionMessage;
                    view.ViewBag.StackTrace = internalServerException.GetExceptionDto().StackTrace;
                    filterContext.Result = view;
                }
                else
                {
                    var view = new ViewResult { ViewName = "Error" };
                    view.ViewBag.Message = inner.Message;
                    view.ViewBag.StackTrace = inner.StackTrace;
                    filterContext.Result = view;
                }
            }

            filterContext.ExceptionHandled = true;
        }

        private void InvalidateUser()
        {
            SessionAttributes.Current().Invalidate();
            FormsAuthentication.SignOut();
        }
    }
}