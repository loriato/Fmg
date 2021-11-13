using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewRelatorioVendaUnificado : BaseEntity
    {
        //Relatorio de vendas unificado
        public virtual string CodigoProposta { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual string PassoAtual { get; set; }
        public virtual string CodigoEmpresa { get; set; }//referente ao empreendimento        
        public virtual string DivisaoEmpreendimento { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string Regional { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual string Estado { get; set; }
        public virtual string CodigoFornecedorSap { get; set; }//referente a empresa de venda
        public virtual string CentralVenda { get; set; }
        public virtual string NomePontoVenda { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual string NomeViabilizador { get; set; }
        public virtual string NomeSupervisor { get; set; }
        public virtual string NomeCoordenador { get; set; }
        public virtual DateTime? DataVenda { get; set; }
        public virtual DateTime? DataComissao { get; set; }
        public virtual Tipologia Tipologia { get; set; }
        public virtual bool FlagFaixaUmMeio { get; set; }
        public virtual string RegraPagamento { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual decimal? ComissaoPagarPNE { get; set; }
        public virtual decimal ComissaoPagarUmMeio { get; set; }
        public virtual decimal ComissaoPagarDois { get; set; }
        public virtual decimal? ValorVGV { get; set; }
        public virtual string StatusRepasse { get; set; }
        public virtual DateTime? DataRepasse { get; set; }
        public virtual bool EmConformidade { get; set; }
        public virtual DateTime? DataConformidade { get; set; }
        public virtual DateTime? DataKitCompleto { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual SituacaoNotaFiscal SituacaoNotaFiscal { get; set; }
        public virtual DateTime? DataPrevisaoPagamento { get; set; }
        public virtual bool Pago { get; set; }
        public virtual string PedidoSap { get; set; }
        public virtual DateTime? DataPedidoSap { get; set; }
        public virtual string NotaFiscal { get; set; }
        public virtual DateTime? DataNotaFiscalEnviada { get; set; }
        public virtual DateTime? DataNotaFiscalRecebida { get; set; }
        public virtual DateTime? DataNotaFiscalAprovada { get; set; }
        public virtual DateTime? DataNotaFiscalReprovada { get; set; }

        //relatório de venda
        public virtual DateTime? DataFaturado { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual string StatusContrato { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual StatusAdiantamentoPagamento AdiantamentoPagamento { get; set; }
        public virtual long IdPontoVenda { get; set; }
        public virtual string NomeFornecedor { get; set; }//referente ao empreendimento

        //Pagamentos
        public virtual string CodigoRegraComissao { get; set; }
        public virtual long IdRegraComissao { get; set; }
        public virtual bool EmReversao { get; set; }
        public virtual string ReciboCompra { get; set; }
        public virtual DateTime? DataRequisicaoCompra { get; set; }
        public virtual string ChamadoPedido { get; set; }
        public virtual string MIRO { get; set; }
        public virtual string MIGO { get; set; }
        public virtual string MIR7 { get; set; }
        public virtual StatusIntegracaoSap? StatusIntegracaoSap { get; set; }
        public virtual string ChamadoPagamento { get; set; }

        
        //Requisição de Compra SAP
        public virtual decimal ValorAPagar { get; set; }
        public virtual string CodigoFornecedor { get; set; }
        public virtual long IdPagamento { get; set; }
        public virtual long IdProposta { get; set; }
        public virtual bool NumeroGerado { get; set; }

        //atualização 15/04/2021
        public virtual string NomeEmpresaVenda { get; set; }

        public virtual TipoEmpresaVenda TipoEmpresaVenda { get; set; }
        public virtual string CnpjEmpresaVenda { get; set; }
        public virtual long IdNotaFiscalPagamento { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
