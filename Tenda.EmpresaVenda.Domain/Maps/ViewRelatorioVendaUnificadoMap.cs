using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewRelatorioVendaUnificadoMap:BaseClassMap<ViewRelatorioVendaUnificado>
    {
        public ViewRelatorioVendaUnificadoMap()
        {
            Table("VW_REL_VENDA_UNIFICADO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_REL_VENDA_UNIFICADO");

            //Relatorio de vendas unificado
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.PassoAtual).Column("DS_PASSO_ATUAL").Nullable();
            Map(reg => reg.CodigoEmpresa).Column("CD_EMPRESA").Nullable();
            Map(reg => reg.DivisaoEmpreendimento).Column("DS_DIVISAO_EMPREENDIMENTO");
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.IdRegional).Column("ID_REGIONAL");
            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.CodigoFornecedorSap).Column("CD_FORNECEDOR_SAP");
            Map(reg => reg.CentralVenda).Column("NM_LOJA");
            Map(reg => reg.NomePontoVenda).Column("NM_PONTO_VENDA");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.NomeViabilizador).Column("NM_VIABILIZADOR");
            Map(reg => reg.NomeSupervisor).Column("NM_SUPERVISOR");
            Map(reg => reg.NomeCoordenador).Column("NM_COORDENADOR");
            Map(reg => reg.DataVenda).Column("DT_VENDA").Nullable();
            Map(reg => reg.DataComissao).Column("DT_COMISSAO").Nullable();
            Map(reg => reg.Tipologia).Column("TP_TIPOLOGIA").CustomType<EnumType<Tipologia>>().Nullable();
            Map(reg => reg.FlagFaixaUmMeio).Column("FL_FAIXA_UM_MEIO").Nullable();
            Map(reg => reg.RegraPagamento).Column("VL_REGRA_PAGAMENTO");
            Map(reg => reg.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>().Nullable();
            Map(reg => reg.ComissaoPagarPNE).Column("VL_COMISSAO_PNE").Nullable();
            Map(reg => reg.ComissaoPagarUmMeio).Column("VL_COMISSAO_UMMEIO").Nullable();
            Map(reg => reg.ComissaoPagarDois).Column("VL_COMISSAO_DOIS").Nullable();
            Map(reg => reg.ValorVGV).Column("VL_VGV").Nullable();
            Map(reg => reg.StatusRepasse).Column("DS_STATUS_REPASSE");
            Map(reg => reg.DataRepasse).Column("DT_REPASSE").Nullable();
            Map(reg => reg.EmConformidade).Column("FL_CONFORMIDADE");
            Map(reg => reg.DataConformidade).Column("DT_CONFORMIDADE").Nullable();
            Map(reg => reg.DataKitCompleto).Column("DT_KIT_COMPLETO").Nullable();
            Map(reg => reg.Faturado).Column("FL_FATURADO");
            Map(reg => reg.SituacaoNotaFiscal).Column("TP_SITUACAO_NOTA_FISCAL").CustomType<EnumType<SituacaoNotaFiscal>>().Nullable();
            Map(reg => reg.DataPrevisaoPagamento).Column("DT_PREVISAO_PAGAMENTO").Nullable();
            Map(reg => reg.Pago).Column("FL_PAGO").Nullable();
            Map(reg => reg.PedidoSap).Column("DS_PEDIDO_SAP").Nullable();
            Map(reg => reg.DataPedidoSap).Column("DT_PEDIDO_SAP").Nullable();
            Map(reg => reg.NotaFiscal).Column("DS_NOTA_FISCAL").Nullable();
            Map(reg => reg.DataNotaFiscalEnviada).Column("DT_NOTA_FISCAL_ENVIADA").Nullable();
            Map(reg => reg.DataNotaFiscalRecebida).Column("DT_NOTA_FISCAL_RECEBIDA").Nullable();
            Map(reg => reg.DataNotaFiscalAprovada).Column("DT_NOTA_FISCAL_APROVADA").Nullable();
            Map(reg => reg.DataNotaFiscalReprovada).Column("DT_NOTA_FISCAL_REPROVADA").Nullable();

            //Relatório de Venda
            Map(reg => reg.DataFaturado).Column("DT_FATURADO").Nullable();
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.StatusContrato).Column("DS_STATUS_CONTRATO");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.AdiantamentoPagamento).Column("TP_STATUS_ADIANTAMENTO").CustomType<EnumType<StatusAdiantamentoPagamento>>();
            Map(reg => reg.IdPontoVenda).Column("ID_PONTO_VENDA");
            Map(reg => reg.NomeFornecedor).Column("NM_EMPRESA");

            //Pagamentos
            Map(reg => reg.CodigoRegraComissao).Column("CD_REGRA_COMISSAO").Nullable();
            Map(reg => reg.IdRegraComissao).Column("ID_REGRA_COMISSAO").Nullable();
            Map(reg => reg.EmReversao).Column("FL_REVERSAO").Nullable();
            Map(reg => reg.ReciboCompra).Column("NR_RECIBO_COMPRA").Nullable();
            Map(reg => reg.DataRequisicaoCompra).Column("DT_REQUISICAO_COMPRA").Nullable();
            Map(reg => reg.ChamadoPedido).Column("DS_CHAMADO_PEDIDO").Nullable();
            Map(reg => reg.MIGO).Column("DS_MIGO").Nullable();
            Map(reg => reg.MIRO).Column("DS_MIRO").Nullable();
            Map(reg => reg.MIR7).Column("NR_MIR7").Nullable();
            Map(reg => reg.ChamadoPagamento).Column("DS_CHAMADO_PAGAMENTO").Nullable();
            Map(reg => reg.StatusIntegracaoSap).Column("TP_STATUS_INTEGRACAO_SAP").CustomType<EnumType<StatusIntegracaoSap>>().Nullable();

            //Requisição de Compra Sap
            Map(reg => reg.IdPagamento).Column("ID_PAGAMENTO").Nullable();
            Map(reg => reg.IdProposta).Column("ID_PROPOSTA_SUAT").Nullable();
            Map(reg => reg.NumeroGerado).Column("FL_NUMERO_GERADO");

            //Atualização 15/04/2021
            Map(reg => reg.NomeEmpresaVenda).Column("NM_FANTASIA").Nullable();

            Map(reg => reg.TipoEmpresaVenda).Column("TP_EMPRESA_VENDA").CustomType<EnumType<TipoEmpresaVenda>>();

            Map(reg => reg.CnpjEmpresaVenda).Column("DS_CNPJ_EMPRESA_VENDA");
            Map(reg => reg.IdNotaFiscalPagamento).Column("ID_NOTA_FISCAL_PAGAMENTO");
        }
    }
}
