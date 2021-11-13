using Europa.Data.Model;
using System;
using System.Net;

namespace Tenda.Domain.Security.Models
{
    public class RequestResponseLog : BaseEntity
    {
        public virtual string RequestMethod { get; set; }
        public virtual DateTime RequestTimestamp { get; set; }
        public virtual string RequestUri { get; set; }
        public virtual string RequestBody { get; set; }
        public virtual HttpStatusCode ResponseStatusCode { get; set; }
        public virtual DateTime ResponseTimestamp { get; set; }
        public virtual string ResponseContentType { get; set; }
        public virtual string ResponseBody { get; set; }
        public virtual string Ip { get; set; }
        public virtual string AuthorizationKey { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
