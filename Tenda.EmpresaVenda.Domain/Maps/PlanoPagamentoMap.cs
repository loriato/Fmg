using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class PlanoPagamentoMap : BaseClassMap<PlanoPagamento>
    {
        public PlanoPagamentoMap()
        {
            Table("TBL_PLANOS_PAGAMENTO");
            Id(reg => reg.Id).Column("ID_PLANO_PAGAMENTO").GeneratedBy.Sequence("SEQ_PLANOS_PAGAMENTO");
            Map(reg => reg.DataVencimento).Column("DT_VENCIMENTO");
            Map(reg => reg.NumeroParcelas).Column("NR_PARCELAS");
            Map(reg => reg.TipoParcela).Column("TP_PARCELA").CustomType<EnumType<SituacaoAprovacaoDocumento>>();
            Map(reg => reg.ValorParcela).Column("VL_PARCELA");
            Map(reg => reg.Total).Column("VL_TOTAL");
            Map(reg => reg.IdSuat).Column("ID_SUAT");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_PLANO_PAGAMENTO_X_PRE_PROPOSTA_01");
        }
    }
}
