using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Resources;

namespace Europa.Extensions
{
    public static class EnumExtensionMethods
    {
        public static string AsString(this Enum value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo == null)
            {
                return string.Empty;
            }
            var attribs = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attribs.IsEmpty())
            {
                return value.ToString();
            }
            var attribute = attribs[0];
            if (attribute.ResourceType != null)
                return new ResourceManager(attribute.ResourceType).GetString(attribute.Name);
            else
                return attribute.Name;
        }
    }
}