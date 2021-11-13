using System;

namespace Europa.Web
{
    public class IgnoreRequestResponseLog : Attribute
    {
        public readonly IgnoreRequestResponseLogType Type;

        /// <summary>
        /// Use IgnoreRequestResponseLogType.All as Default
        /// </summary>
        public IgnoreRequestResponseLog()
        {
            Type = IgnoreRequestResponseLogType.All;
        }

        /// <summary>
        /// To define a custom Ignore Request ResponseLogType
        /// </summary>
        /// <param name="type"></param>
        public IgnoreRequestResponseLog(IgnoreRequestResponseLogType type)
        {
            Type = type;
        }
    }
}
