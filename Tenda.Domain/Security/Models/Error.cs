using Europa.Data.Model;
using System;

namespace Tenda.Domain.Security.Models
{
    public class Error : BaseEntity
    {
        public virtual string Type { get; set; }
        public virtual string Message { get; set; }
        public virtual string Stacktrace { get; set; }
        public virtual string Source { get; set; }
        public virtual long CodeLine { get; set; }
        public virtual string OriginClass { get; set; }
        public virtual string Caller { get; set; }
        public virtual string Target { get; set; }
        public virtual string ContextInfo { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
