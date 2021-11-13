using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;
using Europa.Extensions;

namespace Europa.Web
{
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            ValueProviderResult result = bindingContext.ValueProvider.GetValue(key);
            if (!result.IsNull())
            {
                // the second parameter is the culture to use for conversion.
                // you may want to have a try / catch around the ConvertTo() call,
                // e.g. the user didn't type in a valid date.
                return result.ConvertTo(typeof(Double), CultureInfo.CurrentCulture);
            }
            else
            {
                // no value found
                return null;
            }
        }
    }

    public class NullableDoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            ValueProviderResult result = bindingContext.ValueProvider.GetValue(key);
            if (!result.IsNull())
            {
                // the second parameter is the culture to use for conversion.
                // you may want to have a try / catch around the ConvertTo() call,
                // e.g. the user didn't type in a valid date.
                return string.IsNullOrEmpty(result.AttemptedValue) ? null : (double?)Convert.ToDouble(result.AttemptedValue);
            }
            else
            {
                // no value found
                return null;
            }
        }
    }
    public class TrimModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            var shouldPerformRequestValidation = controllerContext.Controller.ValidateRequest && bindingContext.ModelMetadata.RequestValidationEnabled;
            ValueProviderResult valueResult = GetValueFromValueProvider(bindingContext, shouldPerformRequestValidation);
            //ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == null || string.IsNullOrEmpty(valueResult.AttemptedValue))
            {
                return null;
            }
            return valueResult.AttemptedValue.Trim();
        }

        private ValueProviderResult GetValueFromValueProvider(ModelBindingContext bindingContext, bool performRequestValidation)
        {
            var unvalidatedValueProvider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
            return (unvalidatedValueProvider != null)
              ? unvalidatedValueProvider.GetValue(bindingContext.ModelName, !performRequestValidation)
              : bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        }
    }

    public class InsensitiveModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext,
                                            ModelBindingContext bindingContext,
                                            PropertyDescriptor propertyDescriptor,
                                            object value)
        {
            //only needed if the case was different, in which case value == null
            if (value == null)
            {
                // this does not completely solve the problem, 
                // but was sufficient in my case
                value = bindingContext.ValueProvider.GetValue(
                            bindingContext.ModelName + propertyDescriptor.Name.ToLower());
                var vpr = value as ValueProviderResult;
                if (vpr != null)
                {
                    value = vpr.ConvertTo(propertyDescriptor.PropertyType);
                }
            }
            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }
    }
}