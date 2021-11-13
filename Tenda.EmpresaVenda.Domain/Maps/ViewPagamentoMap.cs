
using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPagamentoMap : BaseClassMap<ViewPagamento>
    {
        public ViewPagamentoMap()
        {
            Table("VW_PAGAMENTOS");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_PAGAMENTO");

            Map(reg => reg.NomeLoja).Column("NM_LOJA").Nullable();
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA").Nullable();
            Map(reg => reg.PassoAtual).Column("DS_PASSO_ATUAL").Nullable();
            Map(reg => reg.DataPassoAtual).Column("DT_PASSO_ATUAL").Nullable();
            Map(reg => reg.DataVenda).Column("DT_VENDA").Nullable();
            Map(reg => reg.Percentual).Column("VL_PERCENTUAL").Nullable();
            Map(reg => reg.RegraPagamento).Column("VL_REGRA_PAGAMENTO").Nullable();
            Map(reg => reg.DataComissao).Column("DT_COMISSAO").Nullable();
            Map(reg => reg.ValorSemPremiada).Column("VL_VGV").Nullable();
            Map(reg => reg.ValorAPagar).Column("VL_A_PAGAR").Nullable();
            Map(reg => reg.EmReversao).Column("FL_REVERSAO").Nullable();
            Map(reg => reg.Pago).Column("FL_PAGO").Nullable();
            Map(reg => reg.PedidoSap).Column("DS_PEDIDO_SAP").Nullable();
            Map(reg => reg.IdPagamento).Column("ID_PAGAMENTO").Nullable();
            Map(reg => reg.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>().Nullable();
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA").Nullable();
            Map(reg => reg.NomeEmpresaVenda).Column("NM_FANTASIA").Nullable();
            Map(reg => reg.IdProposta).Column("ID_PROPOSTA").Nullable();
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO").Nullable();
            Map(reg => reg.FaixaUmMeio).Column("FL_FAIXA_UM_MEIO").Nullable();
            Map(reg => reg.CodigoRegraComissao).Column("CD_REGRA_COMISSAO").Nullable();
            Map(reg => reg.IdRegraComissao).Column("ID_REGRA_COMISSAO").Nullable();
            Map(reg => reg.ReciboCompra).Column("NR_RECIBO_COMPRA").Nullable();
            Map(reg => reg.ChamadoPedido).Column("DS_CHAMADO_PEDIDO").Nullable();
            Map(reg => reg.MIR7).Column("NR_MIR7").Nullable();
            Map(reg => reg.NotaFiscal).Column("DS_NOTA_FISCAL").Nullable();
            Map(reg => reg.ChamadoPagamento).Column("DS_CHAMADO_PAGAMENTO").Nullable();
            Map(reg => reg.DataPrevisaoPagamento).Column("DT_PREVISAO_PAGAMENTO").Nullable();
            Map(reg => reg.Tipologia).Column("TP_TIPOLOGIA").CustomType<EnumType<Tipologia>>().Nullable();
            Map(reg => reg.Estado).Column("DS_ESTADO").Nullable();
            Map(reg => reg.Regional).Column("DS_REGIONAL").Nullable();
            Map(reg => reg.IdRegional).Column("ID_REGIONAL").Nullable();
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO").Nullable();
            Map(reg => reg.NumeroGerado).Column("FL_NUMERO_GERADO");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.SituacaoNotaFiscal).Column("TP_SITUACAO_NOTA_FISCAL").CustomType<EnumType<SituacaoNotaFiscal>>();
            Map(reg => reg.StatusConformidade).Column("DS_STATUS_CONFORMIDADE");
            Map(reg => reg.StatusRepasse).Column("DS_STATUS_REPASSE");
            Map(reg => reg.StatusIntegracaoSap).Column("TP_STATUS_INTEGRACAO_SAP").CustomType<EnumType<StatusIntegracaoSap>>().Nullable();
            Map(reg => reg.DataRequisicaoCompra).Column("DT_REQUISICAO_COMPRA").Nullable();
            Map(reg => reg.DataPedidoSap).Column("DT_PEDIDO_SAP").Nullable();

            Map(reg => reg.DivisaoEmpreendimento).Column("CD_DIVISAO").Nullable();
            Map(reg => reg.CodigoEmpresa).Column("CD_EMPRESA").Nullable();
            Map(reg => reg.CodigoFornecedor).Column("CD_FORNECEDOR_SAP").Nullable();

            Map(reg => reg.Faturado).Column("FL_FATURADO");
            Map(reg => reg.DataFaturado).Column("DT_FATURADO").Nullable();

            Map(reg => reg.MIGO).Column("DS_MIGO").Nullable();
            Map(reg => reg.MIRO).Column("DS_MIRO").Nullable();


        }
    }
}
