using Europa.Data;
using Europa.Data.Model;
using System.Net;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class RequestResponseLogMap : BaseClassMap<RequestResponseLog>
    {
        public RequestResponseLogMap()
        {
            Table("TBL_REQUEST_RESPONSE_LOG");

            Id(reg => reg.Id).Column("ID_REQUEST_RESPONSE_LOG").GeneratedBy.Sequence("SEQ_REQUEST_RESPONSE_LOG");
            Map(reg => reg.RequestBody).Column("DS_REQUEST_BODY").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType);
            Map(reg => reg.RequestMethod).Column("CD_REQUEST_METHOD");
            Map(reg => reg.RequestTimestamp).Column("DT_REQUEST_TIMESTAMP");
            Map(reg => reg.RequestUri).Column("DS_REQUEST_URI");
            Map(reg => reg.ResponseBody).Column("DS_RESPONSE_BODY").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType);
            Map(reg => reg.ResponseContentType).Column("DS_RESPONSE_CONTENT_TYPE");
            Map(reg => reg.ResponseStatusCode).Column("TP_STATUS_CODE").CustomType<HttpStatusCode>();
            Map(reg => reg.ResponseTimestamp).Column("DT_RESPONSE_TIMESTAMP");
            Map(reg => reg.Ip).Column("DS_IP");
            Map(reg => reg.AuthorizationKey).Column("DS_AUTHORIZATION");
        }
    }
}
