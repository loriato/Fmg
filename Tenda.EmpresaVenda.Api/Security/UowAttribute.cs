using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Tenda.EmpresaVenda.Api.Controllers;

namespace Tenda.EmpresaVenda.Api.Security
{
    public class UowAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.Controller is BaseApiController)
            {
                ((BaseApiController)actionContext.ControllerContext.Controller)._session.BeginTransaction();
                base.OnActionExecuting(actionContext);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = ((BaseApiController)actionExecutedContext.ActionContext.ControllerContext.Controller);
            if (actionExecutedContext.Exception == null)
            {
                if (controller._session.Transaction != null && controller._session.Transaction.IsActive)
                {
                    controller._session.Transaction.Commit();
                }
                controller._session.Flush();
                base.OnActionExecuted(actionExecutedContext);
            }
            else
            {
                var transaction = controller._session.Transaction;
                if (transaction != null && transaction.IsActive)
                {
                    transaction.Rollback();
                }
            }
        }
    }
}