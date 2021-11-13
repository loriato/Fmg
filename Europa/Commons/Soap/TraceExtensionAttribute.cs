using System;
using System.Web.Services.Protocols;

namespace Europa.Commons.Soap
{
    public class TraceExtensionAttribute : SoapExtensionAttribute
    {
        private string filename = "c:\\tmp\\europa\\logs\\soaplog.log";
        private int priority;

        public override Type ExtensionType
        {
            get { return typeof(TraceExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
            }
        }
    }
}
