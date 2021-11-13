using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ContratoPrePropostaMap : BaseClassMap<ContratoPreProposta>
    {
        public ContratoPrePropostaMap()
        {
            Table("TBL_CONTRATOS_PRE_PROPOSTA");
            Id(reg => reg.Id).Column("ID_CONTRATO_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_CONTRATOS_PRE_PROPOSTA");
            Map(reg => reg.Contrato).Column("BY_CONTRATO").CustomSqlType(DatabaseStandardDefinitions.LargeObjectCustomType).LazyLoad();
            Map(reg => reg.IdContratoSuat).Column("ID_CONTRATO_SUAT");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_BOLETO_X_PRE_PROPOSTA_01");
        }

    }
}
