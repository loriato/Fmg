using Europa.Extensions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Europa.Commons
{
    public class BusinessRuleException : SystemException
    {
        // FIXME: Deveria ser um Dictionary
        public List<string> Errors = new List<string>();
        public List<string> ErrorsFields = new List<string>();

        private BusinessRuleModel Current;

        public BusinessRuleException()
        {
        }

        public BusinessRuleException(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddError(error.ErrorMessage).AddField(error.PropertyName).Complete();
            }
        }

        public BusinessRuleException(string message) : base(message)
        {
            AddError(message).Complete();
        }

        public BusinessRuleException WithFluentValidation(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddError(error.ErrorMessage).AddField(error.PropertyName).Complete();
            }
            return this;
        }

        public BusinessRuleException AddError(string code)
        {
            Current = new BusinessRuleModel();
            Current.Key = code;
            return this;
        }

        public BusinessRuleException AddField(string field)
        {
            ErrorsFields.Add(field);
            return this;
        }

        public BusinessRuleException WithParam(string parameter)
        {
            Current.AddParameter(new BusinessRuleParameter(parameter, false));
            return this;
        }

        public BusinessRuleException WithParams(params string[] parameters)
        {
            foreach (string param in parameters)
            {
                WithParam(param);
            }
            return this;
        }


        public void Complete()
        {
            if (Current.HasParams())
            {
                Errors.Add(string.Format(Current.Key, Current.Parameters.Select(reg => reg.Value).ToArray()));
            }
            else
            {
                Errors.Add(Current.Key);
            }
            Current = null;
        }

        public bool HasError()
        {
            return !Errors.IsEmpty();
        }

        public void ThrowIfHasError()
        {
            if (HasError())
            {
                throw this;
            }
        }
    }

    public class BusinessRuleModel
    {
        public string Key { get; set; }

        public List<BusinessRuleParameter> Parameters { get; } = new List<BusinessRuleParameter>();

        public bool HasParams()
        {
            return !Parameters.IsNull();
        }

        public void AddParameter(BusinessRuleParameter parameter)
        {
            Parameters.Add(parameter);
        }

    }

    public class BusinessRuleParameter
    {
        public bool IsRawValue { get; set; } = true;
        public string Value { get; set; }

        public BusinessRuleParameter(string value, bool isRawValue)
        {
            Value = value;
            IsRawValue = isRawValue;
        }
    }

}
