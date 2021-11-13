using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Maps
{
    public class SistemaMap : BaseClassMap<Sistema>
    {
        public SistemaMap()
        {
            Table("TBL_SISTEMAS");

            Id(reg => reg.Id).Column("ID_SISTEMA").GeneratedBy.Sequence("SEQ_SISTEMAS");
            Map(reg => reg.Codigo).Column("CD_SISTEMA").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Nome).Column("NM_SISTEMA").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.EnderecoAcesso).Column("DS_ENDERECO_ACESSO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").Nullable().CustomType<EnumType<Situacao>>();
        }
    }
}
