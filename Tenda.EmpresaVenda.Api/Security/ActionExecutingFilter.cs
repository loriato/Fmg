using Europa.Extensions;
using Europa.Web;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Tenda.EmpresaVenda.Api.Controllers;

namespace Tenda.EmpresaVenda.Api.Security
{
    public class ActionExecutingFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as BaseApiController;
            var transactionTypeOfAction = actionContext.ActionDescriptor
                .GetCustomAttributes<TransactionAttribute>(false).FirstOrDefault();
            if (transactionTypeOfAction != null && TransactionAttributeType.Required == transactionTypeOfAction.Type)
            {
                controller.BeginTransaction();
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as BaseApiController;

            if (controller == null)
            {
                return;
            }
            var transactionAlive = !controller.CurrentTransaction().IsNull() && controller.CurrentTransaction().IsActive;

            if (actionExecutedContext.Exception.IsNull() && transactionAlive)
            {
                controller.CurrentTransaction()?.Commit();
            }
            else if (!actionExecutedContext.Exception.IsNull() && transactionAlive)
            {
                controller.CurrentTransaction()?.Rollback();
                controller.CurrentTransaction()?.Dispose();
            }
        }
    }
}