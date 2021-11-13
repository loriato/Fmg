using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class BoletoPrePropostaMap : BaseClassMap<BoletoPreProposta>
    {
        public BoletoPrePropostaMap()
        {
            Table("TBL_BOLETOS_PRE_PROPOSTA");
            Id(reg => reg.Id).Column("ID_BOLETO_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_BOLETOS_PRE_PROPOSTA");
            Map(reg => reg.Boleto).Column("BY_BOLETO").CustomSqlType(DatabaseStandardDefinitions.LargeObjectCustomType).LazyLoad();
            Map(reg => reg.IdBoletoSuat).Column("ID_BOLETO_SUAT");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_BOLETO_X_PRE_PROPOSTA_01");
        }

    }
}
