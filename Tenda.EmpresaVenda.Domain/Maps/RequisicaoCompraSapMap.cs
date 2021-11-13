using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class RequisicaoCompraSapMap : BaseClassMap<RequisicaoCompraSap>
    {
        public RequisicaoCompraSapMap()
        {
            Table("TBL_REQUISICAO_COMPRAS_SAP");

            Id(reg => reg.Id).Column("ID_REQUISICAO_COMPRA_SAP").GeneratedBy.Sequence("SEQ_REUISICAO_COMPRAS_SAP");

            Map(reg => reg.Numero).Column("NR_REQUISICAO_COMPRA").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.Status).Column("DS_STATUS").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>().Nullable();
            Map(reg => reg.Texto).Column("DS_TEXTO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.NumeroGerado).Column("FL_NUMERO_GERADO");

            References(reg => reg.Proposta).Column("ID_PROPOSTA").ForeignKey("FK_REQUISICAO_COMPRA_X_PROPOSTA_01").Nullable();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_REQUISICAO_COMPRA_X_EV_01").Nullable();
        }
    }
}
