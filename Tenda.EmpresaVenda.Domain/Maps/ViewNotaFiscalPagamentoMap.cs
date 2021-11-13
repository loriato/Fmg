using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewNotaFiscalPagamentoMap : BaseClassMap<ViewNotaFiscalPagamento>
    {
        public ViewNotaFiscalPagamentoMap()
        {
            Table("VW_NOTA_FISCAL_PAGAMENTOS");
            ReadOnly();
            SchemaAction.None();

            Id(x => x.Id).Column("ID_VW_REL_COMISSAO");

            Map(x => x.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(x => x.IdNotaFiscalPagamento).Column("ID_NOTA_FISCAL_PAGAMENTO");
            Map(x => x.IdProposta).Column("ID_PROPOSTA");
            Map(x => x.IdArquivo).Column("ID_ARQUIVO");
            Map(x => x.IdPagamento).Column("ID_PAGAMENTO");
            Map(x => x.CodigoProposta).Column("CD_PROPOSTA");
            Map(x => x.DataComissao).Column("DT_COMISSAO");
            Map(x => x.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>();
            Map(x => x.Pago).Column("FL_PAGO");
            Map(x => x.PedidoSap).Column("DS_PEDIDO_SAP");
            Map(x => x.DataPedidoSap).Column("DT_PEDIDO_SAP").Nullable();
            Map(x => x.DataPrevisaoPagamento).Column("DT_PREVISAO_PAGAMENTO");
            Map(x => x.NotaFiscal).Column("DS_NOTA_FISCAL");
            Map(x => x.RegraPagamento).Column("VL_REGRA_PAGAMENTO");
            Map(x => x.SituacaoNotaFiscal).Column("TP_SITUACAO").CustomType<EnumType<SituacaoNotaFiscal>>();
            Map(x => x.NomeFantasia).Column("NM_FANTASIA");
            Map(x => x.Motivo).Column("DS_MOTIVO");
            Map(x => x.Estado).Column("DS_ESTADO");
            Map(x => x.IdRegional).Column("ID_REGIONAL");
            Map(x => x.Regional).Column("DS_REGIONAL");
            Map(x => x.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(x => x.ValorAPagar).Column("VL_A_PAGAR");
            Map(x => x.NomeCliente).Column("NM_CLIENTE");

            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.DescricaoTorre).Column("DS_TORRE");
            Map(reg => reg.DescricaoUnidade).Column("DS_UNIDADE");
            Map(reg => reg.ValorVgv).Column("VL_VGV");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.ModalidadeComissao).Column("TP_MODALIDADE_COMISSAO").CustomType<EnumType<TipoModalidadeComissao>>();
            Map(reg => reg.Tipologia).Column("TP_TIPOLOGIA").CustomType<EnumType<Tipologia>>();
            Map(reg => reg.DataVenda).Column("DT_VENDA");
            Map(reg => reg.Percentual).Column("VL_PERCENTUAL");
            Map(reg => reg.EmReversao).Column("FL_REVERSAO");
            Map(reg => reg.PassoAtual).Column("DS_PASSO_ATUAL");

            Map(reg => reg.Faturado).Column("FL_FATURADO");
            Map(reg => reg.DataFaturado).Column("DT_FATURADO").Nullable();
            Map(reg => reg.RevisaoNF).Column("NR_ENVIO_NOTA_FISCAL").Nullable();

            Map(reg => reg.CnpjEmpresaVenda).Column("DS_CNPJ_EMPRESA_VENDA");
        }
    }
}
