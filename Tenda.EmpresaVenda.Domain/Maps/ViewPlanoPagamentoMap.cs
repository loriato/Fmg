using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ViewPlanoPagamentoMap : BaseClassMap<ViewPlanoPagamento>
    {
        public ViewPlanoPagamentoMap()
        {
            Table("VW_PLANOS_PAGAMENTO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_PLANO_PAGAMENTO");
            Map(reg => reg.DataVencimento).Column("DT_VENCIMENTO");
            Map(reg => reg.NumeroParcelas).Column("NR_PARCELAS");
            Map(reg => reg.TipoParcela).Column("TP_PARCELA").CustomType<EnumType<SituacaoAprovacaoDocumento>>();
            Map(reg => reg.ValorParcela).Column("VL_PARCELA");
            Map(reg => reg.Total).Column("VL_TOTAL");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
        }
    }
}
