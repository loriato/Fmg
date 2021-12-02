using Europa.Extensions;
using NHibernate;
using System;
using System.IO;
using System.Web.Services.Protocols;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Shared.Log;

namespace PortalPosVenda.Domain.Commons
{
    public class SoapTraceLogger : SoapExtension
    {
        private Stream _originalStream;
        private Stream _workingStream;
        private string _endpoit;

        public override Stream ChainStream(Stream stream)
        {
            _originalStream = stream;
            _workingStream = new MemoryStream();
            return _workingStream;
        }

        public override object GetInitializer(Type serviceType)
        {
            return serviceType.Name;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return "GetInitializer-" + attribute.ToString();
        }

        public override void Initialize(object initializer)
        {
            _endpoit = (string)initializer;
        }

        public override void ProcessMessage(SoapMessage message)
        {
            string content = null;
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    content = GetSoapEnvelope(message.Stream);
                    CopyStream(_workingStream, _originalStream);
                    LogRequest(message, content);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(_originalStream, _workingStream);
                    content = GetSoapEnvelope(message.Stream);
                    LogRequest(message, content);
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
            }
        }

        private void LogRequest(SoapMessage soapMessage, string content)
        {
            try
            {
                WebServiceMessage message = new WebServiceMessage();
                message.Stage = (Tenda.Domain.Security.Enums.SoapMessageStage)soapMessage.Stage.GetHashCode();
                message.Content = content;
                message.Endpoint = _endpoit;
                message.Action = soapMessage.MethodInfo.Name;
                IStatelessSession session = NHibernateSession.StatelessSession();
                session.Insert(message);
                session.CloseIfOpen();
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }

        private string GetSoapEnvelope(Stream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string data = reader.ReadToEnd();
            stream.Position = 0;
            return data;
        }

        private void CopyStream(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }

    }
}
