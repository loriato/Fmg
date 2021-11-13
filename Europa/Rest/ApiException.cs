using System;
using System.Collections.Generic;
using Europa.Extensions;
using Europa.Web;
using FluentValidation.Results;

namespace Europa.Rest
{
    public class ApiException : Exception
    {
        private BaseResponse Response = new BaseResponse();

        public ApiException()
        {
        }

        public ApiException(BaseResponse response)
        {
            Response = response;
        }

        public ApiException(string message) : base(message)
        {
            Response.Messages.Add(message);
        }

        public ApiException WithFluentValidation(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddValidationError(error);
            }

            return this;
        }

        private void AddValidationError(ValidationFailure error)
        {
            if (error.PropertyName.Contains(RestDefinitions.GlobalMessageErrorKey))
            {
                Response.Messages.Add(error.ErrorMessage);
            }
            else
            {
                Response.Fields.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
            }
        }

        public void AddError(string message)
        {
            Response.Messages.Add(message);
        }

        public void AddError(List<string> messages)
        {
            Response.Messages.AddRange(messages);
        }

        public void AddErrors(List<string> messages)
        {
            Response.Messages.AddRange(messages);
        }

        public void AddError(string key, string message)
        {
            Response.Fields.Add(new KeyValuePair<string, string>(key, message));
        }

        public bool HasError()
        {
            return !Response.Messages.IsEmpty() || !Response.Fields.IsEmpty();
        }

        public void ThrowIfHasError()
        {
            if (HasError())
            {
                throw this;
            }
        }

        public BaseResponse GetResponse()
        {
            return Response;
        }
    }
}
