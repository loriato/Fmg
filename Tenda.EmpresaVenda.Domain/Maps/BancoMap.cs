using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class BancoMap : BaseClassMap<Banco>
    {
        public BancoMap()
        {
            Table("TBL_BANCOS");
            Id(reg => reg.Id).Column("ID_BANCO").GeneratedBy.Sequence("SEQ_BANCOS");
            Map(reg => reg.Codigo).Column("CD_BANCO").Length(DatabaseStandardDefinitions.TenLength).Nullable();
            Map(reg => reg.Nome).Column("NM_BANCO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Sigla).Column("DS_SIGLA").Length(DatabaseStandardDefinitions.TwentyLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<Situacao>();
        }
    }
}
