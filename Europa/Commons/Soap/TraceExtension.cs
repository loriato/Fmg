using System;
using System.IO;
using System.Web.Services.Protocols;

namespace Europa.Commons.Soap
{
    public class TraceExtension : SoapExtension
    {
        Stream oldStream;
        Stream newStream;
        string filename;

        // Save the Stream representing the SOAP request or SOAP response into
        // a local memory buffer.
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        // When the SOAP extension is accessed for the first time, the XML Web
        // service method it is applied to is accessed to store the file
        // name passed in, using the corresponding SoapExtensionAttribute.
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return ((TraceExtensionAttribute)attribute).Filename;
        }

        // The SOAP extension was configured to run using a configuration file
        // instead of an attribute applied to a specific XML Web service
        // method.
        public override object GetInitializer(Type serviceType)
        {
            // Return a file name to log the trace information to, based on the
            // type.

            return "C:\\tmp\\europa\\logs\\" + serviceType.Name + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        }

        // Receive the file name stored by GetInitializer and store it in a
        // member variable for this specific instance.
        public override void Initialize(object initializer)
        {
            filename = (string)initializer;
        }

        //  If the SoapMessageStage is such that the SoapRequest or
        //  SoapResponse is still in the SOAP format to be sent or received,
        //  save it out to a file.
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                case SoapMessageStage.AfterDeserialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    WriteOutput(message);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    WriteInput(message);
                    break;
            }
        }

        public void WriteOutput(SoapMessage message)
        {
            newStream.Position = 0;
            FileStream fs = new FileStream(filename, FileMode.Append,
                FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);

            string soapString = (message is SoapServerMessage) ? "SoapResponse" : "SoapRequest";
            w.WriteLine("-----" + soapString + " at " + DateTime.Now);
            w.Flush();
            Copy(newStream, fs);
            w.Close();
            newStream.Position = 0;
            Copy(newStream, oldStream);
        }

        public void WriteInput(SoapMessage message)
        {
            Copy(oldStream, newStream);
            FileStream fs = new FileStream(filename, FileMode.Append,
                FileAccess.Write);
            StreamWriter w = new StreamWriter(fs);

            string soapString = (message is SoapServerMessage) ?
                "SoapRequest" : "SoapResponse";
            w.WriteLine("-----" + soapString +
                " at " + DateTime.Now);
            w.Flush();
            newStream.Position = 0;
            Copy(newStream, fs);
            w.Close();
            newStream.Position = 0;
        }

        void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }
    }
}
