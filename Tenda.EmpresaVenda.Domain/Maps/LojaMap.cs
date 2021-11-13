using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class LojaMap : BaseClassMap<Loja>
    {
        public LojaMap()
        {
            Table("TBL_LOJAS");

            Id(reg => reg.Id).Column("ID_LOJA").GeneratedBy.Sequence("SEQ_LOJAS");
            Map(reg => reg.Nome).Column("NM_LOJA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.NomeFantasia).Column("DS_NOME_FANTASIA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.SapId).Column("ID_SAP").Length(DatabaseStandardDefinitions.TwentyLength);
            Map(reg => reg.DataIntegracao).Column("DT_INTEGRACAO");
            Map(reg => reg.Situacao).Column("DS_SITUACAO").CustomType<EnumType<TipoContato>>();
            Map(reg => reg.NomeRegional).Column("NM_REGIONAL").Length(DatabaseStandardDefinitions.TenLength).Nullable();
            References(reg => reg.Regional).Column("ID_REGIONAL").ForeignKey("FK_LOJAS_X_REGIONAIS_01");

        }
    }
}
