using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using Europa.Commons;
using Europa.Rest;
using Europa.Web;
using Newtonsoft.Json;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Api.Controllers;

namespace Tenda.EmpresaVenda.Api.Security
{
    public sealed class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as BaseApiController;
            var transaction = controller?.CurrentTransaction();
            if (transaction != null && transaction.IsActive)
            {
                transaction.Rollback();
            }

            var response = new BaseResponse();
            if (actionExecutedContext.Exception is ApiException apiException)
            {
                response = apiException.GetResponse();
                response.Code = (int)HttpStatusCode.BadRequest;
                actionExecutedContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8,
                        "application/json"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else if (actionExecutedContext.Exception is BusinessRuleException bsrException)
            {
                response.Code = (int)HttpStatusCode.BadRequest;
                response.Messages = bsrException?.Errors;
                actionExecutedContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8,
                        "application/json"),
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
            else
            {
                var exceptionDto = new ExceptionDto();
                exceptionDto.Message = actionExecutedContext.Exception.Source;
                exceptionDto.ExceptionMessage = actionExecutedContext.Exception.Message;
                exceptionDto.StackTrace = actionExecutedContext.Exception.StackTrace;
                var errors = new List<string>();
                LogWhileHasInnerException(actionExecutedContext.Exception, ref errors);

                actionExecutedContext.Response = new HttpResponseMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(exceptionDto), Encoding.UTF8,
                        "application/json"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
                return;
            }

            actionExecutedContext.Response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "application/json"),
                StatusCode = (HttpStatusCode)response.Code
            };
        }

        private void LogWhileHasInnerException(Exception exception, ref List<string> errors)
        {
            if (exception.InnerException != null)
            {
                LogWhileHasInnerException(exception.InnerException, ref errors);
            }

            errors.Add(exception.Message);
            ExceptionLogger.LogException(exception);
        }
    }
}