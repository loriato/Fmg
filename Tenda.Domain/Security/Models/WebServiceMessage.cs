using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class WebServiceMessage : BaseEntity
    {
        public virtual string Endpoint { get; set; }
        public virtual string Action { get; set; }
        public virtual string Source { get; set; }
        public virtual string Content { get; set; }
        public virtual SoapMessageStage Stage { get; set; }
        public virtual string TransactionCode { get; set; }
        public virtual bool Reprocessed { get; set; }
        public virtual bool DataExtracted { get; set; }
        public virtual string ClienteSap { get; set; }
        public virtual string InformacaoContextual { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }

        public virtual WebServiceMessage DeepCopy()
        {
            WebServiceMessage copy = new WebServiceMessage();
            copy.Endpoint = Endpoint;
            copy.Action = Action;
            copy.Source = Source;
            copy.Content = Content;
            copy.Stage = Stage;
            copy.TransactionCode = TransactionCode;
            copy.Reprocessed = Reprocessed;
            copy.DataExtracted = DataExtracted;
            copy.ClienteSap = ClienteSap;
            copy.InformacaoContextual = InformacaoContextual;

            return copy;
        }
    }
}
