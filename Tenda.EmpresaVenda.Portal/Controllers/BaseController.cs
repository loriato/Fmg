using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Web;
using Newtonsoft.Json;
using NHibernate;
using PortalPosVenda.Domain.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using Tenda.Domain.Core.Data;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Europa.Fmg.Portal.Models.Application;
using Europa.Fmg.Portal.Security;

namespace Europa.Fmg.Portal.Controllers
{

    [BaseAuthorize(true)]
    [Transaction(TransactionAttributeType.None)]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class BaseController : Controller
    {
        protected ISession _session { get; set; }
        protected ITransaction _transaction;
        private List<string> _errorMessages;
        private List<string> _successMessages;
        public RequestState _requestState { get; set; }

        protected BaseController()
        {
        }

        protected BaseController(ISession session)
        {
            this._session = session;
        }

        protected ISession CurrentSession()
        {
            return _session;
        }

        protected ITransaction CurrentTransaction()
        {
            return _transaction;
        }

        protected void AddSuccessMessage(string message)
        {
            _successMessages.Add(message);
        }

        protected void ClearMessages()
        {
            _successMessages = new List<String>();
            _errorMessages = new List<String>();
        }

        protected void AddErrorMessage(string message)
        {
            _errorMessages.Add(message);
        }

        protected bool HasErrorMessage()
        {
            return _errorMessages.IsEmpty() == false;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (!_transaction.IsNull() && _transaction.IsActive)
            {
                _transaction.Rollback();
            }

            string sessionAttributes = "";
            try
            {
                sessionAttributes = JsonConvert.SerializeObject(SessionAttributes.Current(), Formatting.None, SecurityEntitySerializer.Current());
            }
            catch (Exception e)
            {
                sessionAttributes = e.Message;
            }

            long errorId = ExceptionLogger.LogException(exc: filterContext.Exception, contextInfo: sessionAttributes);

            var inner = filterContext.Exception;
            while (inner.InnerException != null)
            {
                inner = inner.InnerException;
            }

            filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //FIXME: Use ViewModel to respond. 
                //FIXME: Use HTTP error codes instead Success
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Success = false,
                        Message = inner.Message,
                        StackTrace = inner.StackTrace,
                        ErrorId = errorId
                    }
                };
                filterContext.ExceptionHandled = true;
            }
            else
            {
                //FIXME: Verificar como usar o System.Web.Mvc.HandleErrorInfo para tratar isso.
                var view = new ViewResult { ViewName = "Error" };
                view.ViewBag.Message = inner.Message;
                view.ViewBag.StackTrace = inner.StackTrace;
                view.ViewBag.ErrorId = errorId;
                filterContext.Result = view;
            }
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            this.ClearMessages();

            TransactionAttribute transactionTypeOfAction = (TransactionAttribute)filterContext.ActionDescriptor.GetCustomAttributes(typeof(TransactionAttribute), false).FirstOrDefault();

            if (transactionTypeOfAction != null && TransactionAttributeType.Required == transactionTypeOfAction.Type)
            {
                _transaction = CurrentSession().BeginTransaction();
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            bool transactionAlive = !_transaction.IsNull() && _transaction.IsActive;

            if (filterContext.Exception.IsNull() && transactionAlive)
            {
                _transaction.Commit();
            }
            else if (!filterContext.Exception.IsNull() && transactionAlive)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }

            ViewBag.ErrorMessages = _errorMessages;
            ViewBag.SuccessMessages = _successMessages;

            if (!ApplicationInfo.IsProductionMode())
            {
                DesabilitarCacheBrowser(filterContext);
            }
        }

        private void DesabilitarCacheBrowser(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Buffer = true;
            filterContext.HttpContext.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            filterContext.HttpContext.Response.Expires = 0;
            filterContext.HttpContext.Response.CacheControl = "no-cache";
        }

        public string RenderRazorViewToString(string viewName, object model, bool detailMode)
        {
            ViewData.Model = model;

            ViewBag.ErrorMessages = _errorMessages;
            ViewBag.SuccessMessages = _successMessages;


            if (!model.IsNull() && detailMode)
            {
                ViewData.Model = model;
                ViewData.Add("detailMode", true);
            }

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                var aux = sw.GetStringBuilder().ToString();
                return aux;
            }
        }

        protected List<string> ModelStateErrors(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            return query.ToList();
        }

        protected void HandleBusinessRuleException(BusinessRuleException exc)
        {
            _errorMessages.AddRange(exc.Errors);
        }

        protected string GetWebAppRoot()
        {
            // Solução para o caso do site estar como HTTP no IIS e o BigIP que controla o HTTPS.
            // No PdfJs, o parametro é passado errado e ele não deixa o Host passar
            // https://stackoverflow.com/questions/37378251/load-pdf-on-foreign-url-with-pdf-js
            string parameterWebRoot = ProjectProperties.EvsBaseUrl;
            if (!parameterWebRoot.IsEmpty())
            {
                return parameterWebRoot;
            }

            var host = (Request.Url.IsDefaultPort) ?
                Request.Url.Host :
                Request.Url.Authority;
            host = string.Format("{0}://{1}", Request.Url.Scheme, host);
            if (Request.ApplicationPath == "/")
                return host;
            else
                return host + Request.ApplicationPath;
        }

        protected string GetWebAppRootAdmin()
        {
            // Solução para o caso do site estar como HTTP no IIS e o BigIP que controla o HTTPS.
            // No PdfJs, o parametro é passado errado e ele não deixa o Host passar
            // https://stackoverflow.com/questions/37378251/load-pdf-on-foreign-url-with-pdf-js
            string parameterWebRoot = ProjectProperties.EvsAdminBaseUrl;
            if (!parameterWebRoot.IsEmpty())
            {
                return parameterWebRoot;
            }
            var host = (Request.Url.IsDefaultPort) ?
                Request.Url.Host :
                Request.Url.Authority;
            host = string.Format("{0}://{1}", Request.Url.Scheme, host);
            if (Request.ApplicationPath == "/")
                return host;
            else
                return host + Request.ApplicationPath;
        }
    }
}