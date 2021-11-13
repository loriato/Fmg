using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class WebServiceMessageMap : BaseClassMap<WebServiceMessage>
    {
        public WebServiceMessageMap()
        {
            Table("TBL_WEB_SERVICE_MESSAGES");

            Id(reg => reg.Id).Column("ID_WEB_SERVICE_MESSAGE").GeneratedBy.Sequence("SEQ_WEB_SERVICE_MESSAGES");
            Map(reg => reg.Endpoint).Column("DS_ENDPOINT").Not.Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Action).Column("DS_ACTION").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Source).Column("DS_SOURCE").Length(DatabaseStandardDefinitions.FortyLength);
            Map(reg => reg.Stage).Column("TP_STAGE").Not.Nullable().CustomType<EnumType<SoapMessageStage>>();
            Map(reg => reg.Content).Column("DS_CONTENT").CustomSqlType(DatabaseStandardDefinitions.LargeStringCustomType);
            Map(reg => reg.Reprocessed).Column("FL_REPROCESSED");
            Map(reg => reg.TransactionCode).Column("CD_TRANSACTION").Length(DatabaseStandardDefinitions.FiftyLength);
            Map(reg => reg.DataExtracted).Column("FL_DATA_EXTRACTED");
            Map(reg => reg.ClienteSap).Column("CD_CLIENTE_SAP").Length(DatabaseStandardDefinitions.TwentyLength);
            Map(reg => reg.InformacaoContextual).Column("CD_CONTEXTUAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
        }
    }
}
