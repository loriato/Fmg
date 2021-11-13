using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewRelatorioComissaoMap : BaseClassMap<ViewRelatorioComissao>
    {
        public ViewRelatorioComissaoMap()
        {
            Table("VW_REL_COMISSAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_REL_COMISSAO");

            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.IdRegional).Column("ID_REGIONAL");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.Divisao).Column("DS_DIVISAO_EMPREENDIMENTO");
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.CentralVenda).Column("NM_LOJA");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.DataSicaq).Column("DT_SICAQ").Nullable();
            Map(reg => reg.StatusContrato).Column("DS_STATUS_CONTRATO");
            Map(reg => reg.StatusRepasse).Column("DS_STATUS_REPASSE");
            Map(reg => reg.DataRepasse).Column("DT_REPASSE").Nullable();
            Map(reg => reg.StatusConformidade).Column("DS_STATUS_CONFORMIDADE");
            Map(reg => reg.DataConformidade).Column("DT_CONFORMIDADE").Nullable();
            Map(reg => reg.RegraPagamento).Column("VL_REGRA_PAGAMENTO");
            Map(reg => reg.CodigoFornecedor).Column("CD_EMPRESA");
            Map(reg => reg.NomeFornecedor).Column("NM_EMPRESA");
            Map(reg => reg.NomeEmpresaVenda).Column("ID_SAP_ESTABELECIMENTO");
            Map(reg => reg.DescricaoTorre).Column("DS_TORRE");
            Map(reg => reg.DescricaoUnidade).Column("DS_UNIDADE");
            Map(reg => reg.DataVenda).Column("DT_VENDA").Nullable();
            Map(reg => reg.ValorVGV).Column("VL_VGV").Nullable();
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA");
            Map(reg => reg.DescricaoTipologia).Column("DS_TIPOLOGIA");
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.FaixaUmMeio).Column("VL_ACORDADO_UM_MEIO");
            Map(reg => reg.FaixaDois).Column("VL_ACORDADO_DOIS");
            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.IdRegraComissaoEvs).Column("ID_REGRA_COMISSAO_EVS");
            Map(reg => reg.DataRegistro).Column("DT_REGISTRO").Nullable();
            Map(reg => reg.Observacao).Column("DS_OBSERVACAO");
            Map(reg => reg.ComissaoPagarDois).Column("VL_COMISSAO_DOIS").Nullable();
            Map(reg => reg.ComissaoPagarUmMeio).Column("VL_COMISSAO_UMMEIO").Nullable();
            Map(reg => reg.CodigoRegra).Column("CD_REGRA_COMISSAO").Nullable();
            Map(reg => reg.IdRegraComissao).Column("ID_REGRA_COMISSAO");
            Map(reg => reg.DataKitCompleto).Column("DT_KIT_COMPLETO").Nullable();
            Map(reg => reg.PassoAtual).Column("DS_PASSO_ATUAL").Nullable();
            //Map(reg => reg.ValorAPagar).Column("VL_A_PAGAR").Nullable();
            Map(reg => reg.EmReversao).Column("FL_REVERSAO").Nullable();
            //Map(reg => reg.PedidoSap).Column("DS_PEDIDO_SAP").Nullable();
            //Map(reg => reg.DataPassoAtual).Column("DT_PASSO_ATUAL").Nullable();
            Map(reg => reg.FlagFaixaUmMeio).Column("FL_FAIXA_UM_MEIO").Nullable();
            Map(reg => reg.EmpresaDeVenda).Column("NM_FANTASIA").Nullable();
            Map(reg => reg.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>().Nullable();
            //Map(reg => reg.IdPagamento).Column("ID_PAGAMENTO").Nullable();
            //Map(reg => reg.IdItemRegraComissao).Column("ID_ITEM_REGRA_COMISSAO").Nullable();
            Map(reg => reg.IdProposta).Column("ID_PROPOSTA_SUAT").Nullable();
            //Map(reg => reg.DataComissao).Column("DT_COMISSAO").Nullable();
            Map(reg => reg.Pago).Column("FL_PAGO").Nullable();
            Map(reg => reg.DataPagamento).Column("DT_PAGAMENTO").Nullable();
            Map(reg => reg.KitCompleto).Column("FL_KIT_COMPLETO");
            Map(reg => reg.AdiantamentoPagamento).Column("TP_STATUS_ADIANTAMENTO").CustomType<EnumType<StatusAdiantamentoPagamento>>();
            Map(reg => reg.Tipologia).Column("TP_TIPOLOGIA").CustomType<EnumType<Tipologia>>().Nullable();
            Map(reg => reg.Faixa).Column("VL_FAIXA").Nullable();
            Map(reg => reg.Modalidade).Column("TP_MODALIDADE_COMISSAO").CustomType<EnumType<TipoModalidadeComissao>>();
            Map(reg => reg.ComissaoPagarPNE).Column("VL_COMISSAO_PNE").Nullable();
            Map(reg => reg.IdPontoVenda).Column("ID_PONTO_VENDA").Nullable();
            Map(reg => reg.NomePontoVenda).Column("NM_PONTO_VENDA").Nullable();
            Map(reg => reg.Faturado).Column("FL_FATURADO");
            Map(reg => reg.DataFaturado).Column("DT_FATURADO").Nullable();
            Map(reg => reg.DataSicaqPreproposta).Column("DT_SICAQ_PPR").Nullable();

            Map(reg=>reg.TipoEmpresaVenda).Column("TP_EMPRESA_VENDA").CustomType<EnumType<TipoEmpresaVenda>>();
        }
    }
}
