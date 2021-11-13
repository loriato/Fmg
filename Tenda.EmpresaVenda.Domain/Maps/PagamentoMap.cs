using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PagamentoMap : BaseClassMap<Pagamento>
    {
        public PagamentoMap()
        {
            Table("TBL_PAGAMENTOS");

            Id(reg => reg.Id).Column("ID_PAGAMENTO").GeneratedBy.Sequence("SEQ_PAGAMENTOS");

            Map(reg => reg.PedidoSap).Column("DS_PEDIDO_SAP").Nullable();
            Map(reg => reg.ValorPagamento).Column("VL_PAGAMENTO").Nullable();
            Map(reg => reg.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>().Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>().Not.Nullable();
            Map(reg => reg.ReciboCompra).Column("NR_RECIBO_COMPRA").Nullable();
            Map(reg => reg.ChamadoPedido).Column("DS_CHAMADO_PEDIDO").Nullable();
            Map(reg => reg.MIR7).Column("NR_MIR7").Nullable();
            Map(reg => reg.NotaFiscal).Column("DS_NOTA_FISCAL").Nullable();
            Map(reg => reg.ChamadoPagamento).Column("DS_CHAMADO_PAGAMENTO").Nullable();
            Map(reg => reg.DataPrevisaoPagamento).Column("DT_PREVISAO_PAGAMENTO").Nullable();
            Map(reg => reg.Pago).Column("FL_PAGO").Not.Nullable();
            Map(reg => reg.StatusIntegracaoSap).Column("TP_STATUS_INTEGRACAO_SAP").CustomType<EnumType<StatusIntegracaoSap>>().Nullable();
            Map(reg => reg.DataRequisicaoCompra).Column("DT_REQUISICAO_COMPRA").Nullable();
            Map(reg => reg.DataPedidoSap).Column("DT_PEDIDO_SAP").Nullable();

            Map(reg => reg.MIGO).Column("DS_MIGO").Nullable();
            Map(reg => reg.MIRO).Column("DS_MIRO").Nullable();

            References(reg => reg.Proposta).Column("ID_PROPOSTA").ForeignKey("FK_PAGAMENTO_X_PROPOSTA_01");
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_PAGAMENTO_X_XEMPRESA_VENDA_01");
            References(reg => reg.NotaFiscalPagamento).Column("ID_NOTA_FISCAL_PAGAMENTO").ForeignKey("FK_PAGAMENTO_X_NOTA_FISCAL_01").Nullable();
        }
    }
}