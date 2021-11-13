using Europa.Extensions;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Simulador
{
    public class SimuladorDto: FilterDto
    {
        public virtual DateTime CriadoEm { get; set; }
        public virtual string Codigo { get; set; }
        public virtual string Digito { get; set; }
        public virtual bool PlanoFinalImpresso { get; set; }
        public virtual decimal AvaliacaoPotencial { get; set; }
        public virtual bool FaixaUmMeioPotencial { get; set; }
        public virtual decimal CotaFinanciamento { get; set; }
        public virtual decimal Subsidio { get; set; }
        public virtual decimal FinanciamentoPreAprovado { get; set; }
        public virtual decimal FinanciamentoPreAprovadoSolicitado { get; set; }
        public virtual bool FinanciamentoEditado { get; set; }
        public virtual int PrazoAmortizacao { get; set; }
        public virtual decimal TaxaJuros { get; set; }
        public virtual int PrazoObraCef { get; set; }
        public virtual decimal PrimeiraParcelaFinanciamento { get; set; }
        public virtual bool TetoEmpreendimentoIgnorado { get; set; }
        public virtual bool FaixaDoisForcado { get; set; }


        // Não exibido em tela
        public virtual decimal TaxaAdministrativa { get; set; }
        public virtual decimal ParcelaBruta { get; set; }
        public virtual decimal DFI { get; set; }
        public virtual decimal ParcelaMip { get; set; }
        public virtual decimal Mip { get; set; }
        public virtual decimal ParcelaLiquida { get; set; }
        public virtual decimal TotalFinanciado { get; set; }
        public virtual decimal SubsidioFaixaUmMeio { get; set; }
        public virtual decimal SubsidioFaixaDois { get; set; }

        // Informações de Entrada e FGTS
        public virtual decimal ValorEntrada { get; set; }
        public virtual decimal ValorFgts { get; set; }
        public virtual decimal GanhoEntrada { get; set; }

        // Informações de Pré-Chaves e Intermediária
        public virtual decimal ValorTotalPreChaves { get; set; }
        public virtual decimal ValorTotalIntermediaria { get; set; }
        public virtual decimal ValorTotalAprovadoCliente { get; set; }
        public virtual decimal GanhoPreChaves { get; set; }
        public virtual int QuantidadeParcelaAprovadaPreChaves { get; set; }
        public virtual decimal ValorParcelaAprovadaPreChaves { get; set; }
        public virtual decimal ValorTotalAprovadoPreChaves { get; set; }
        public virtual int QuantidadeParcelaNegociadaPreChaves { get; set; }
        public virtual decimal ValorParcelaNegociadaPreChaves { get; set; }
        public virtual decimal ValorTotalNegociadoPreChaves { get; set; }
        public virtual decimal ValorTotalAprovadoIntermediaria { get; set; }
        public virtual DateTime? DataLimiteIntermediaria { get; set; }
        public virtual decimal ValorPrimeiraIntermediaria { get; set; }
        public virtual DateTime? DataPrimeiraIntermediaria { get; set; }
        public virtual decimal TotalNegociadoIntermediaria { get; set; }
        public virtual decimal ValorSegundaIntermediaria { get; set; }
        public virtual DateTime? DataSegundaIntermediaria { get; set; }
        public virtual decimal TotalPreChavesIntermediaria { get; set; }

        // Informações de Pós-Chaves
        public virtual DateTime? InicioPagamentoPosChaves { get; set; }
        public virtual int QuantidadeParcelaAprovadaPosChaves { get; set; }
        public virtual decimal ValorParcelaAprovadaPosChaves { get; set; }
        public virtual decimal ValorTotalAprovadoPosChaves { get; set; }
        public virtual decimal GanhoPosChaves { get; set; }
        public virtual int QuantidadeParcelaNegociadaPosChaves { get; set; }
        public virtual decimal ValorParcelaNegociadaPosChaves { get; set; }
        public virtual decimal ValorTotalNegociadoPosChaves { get; set; }

        // Informações de ITBI e Registro
        public virtual decimal ValorEmolumento { get; set; }
        public virtual decimal ValorItbi { get; set; }
        public virtual decimal ValorTotal { get; set; }
        public virtual bool PagamentoItbiParcelado { get; set; }
        public virtual int QuantidadeParcelaPreChavesItbi { get; set; }
        public virtual decimal ValorParcelaPreChavesItbi { get; set; }
        public virtual decimal ValorTotalPreChavesItbi { get; set; }
        public virtual int QuantidadeParcelaIntermediariaItbi { get; set; }
        public virtual decimal ValorParcelaIntermediariaItbi { get; set; }
        public virtual decimal ValorTotalIntermediariaItbi { get; set; }
        public virtual int QuantidadeParcelaPosChavesItbi { get; set; }
        public virtual decimal ValorParcelaPosChavesItbi { get; set; }
        public virtual decimal ValorTotalPosChavesItbi { get; set; }

        // Informações sobre Plano Final
        public virtual decimal ValorAvaliacaoCaixa { get; set; }
        public virtual decimal ValorDescontoTenda { get; set; }
        public virtual decimal ValorContrato { get; set; }
        public virtual decimal ValorParcelaPremiadaTenda { get; set; }
        public virtual decimal ValorPrecoFinalCliente { get; set; }
        public virtual decimal ValorTotalPreChavesPlanoFinal { get; set; }
        public virtual decimal ValorTotalPosChavesPlanoFinal { get; set; }
        public virtual decimal ValorTotalDescontoCliente { get; set; }

        //Complemento do Cliente
        public virtual DateTime DataVencimentoEntrada { get; set; }
        public virtual DateTime DataVencimentoPre { get; set; }
        public virtual DateTime DataVencimentoPos { get; set; }
        public virtual DateTime DataVencimentoIntermediaria { get; set; }
        public virtual DateTime DataVencimentoPreItbi { get; set; }
        public virtual DateTime DataVencimentoPosItbi { get; set; }

        //atualização
        public virtual bool EditadaPlanoFinal { get; set; }

        // Informações de Financiamento
        public virtual DateTime DataEntregaCef { get; set; }
        public virtual decimal PrimeiraParcelaFinanciamentoSolicitado { get; set; }

        // Informações de Entrada e FGTS
        public virtual bool FgtsZerado { get; set; }

        // Informações de Pré-Chaves e Intermediária
        public virtual DateTime? DataVencimentoPrimeiraIntermediaria { get; set; }
        public virtual DateTime? DataVencimentoSegundaIntermediaria { get; set; }
        public virtual bool PreChavesZerado { get; set; }
        public virtual bool PreChavesIntermediariaZerado { get; set; }

        // Informações de Pós-Chaves
        public virtual bool PosChavesZerado { get; set; }

        // Dados SUAT
        public virtual long? IdProposta { get; set; }

        // Registro de Revisão da Simulação

        // Dados do Score Serasa
        public virtual decimal ScoreSerasaCliente { get; set; }

        // Dados EVS
        public virtual string CodigoPreProposta { get; set; }

        // Dados Alçada
        public virtual string JustificativaAlcada { get; set; }


        //Info unidade
        public virtual bool Reservada { get; set; }

        //Info Cliente
        public virtual string NomeCliente { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Email { get; set; }
        public virtual TipoEstadoCivil? EstadoCivil { get; set; }
        public virtual DateTime? DataNascimento { get; set; }
        public virtual string Cpf { get; set; }

        //Torre
        public string Torre { get; set; }
        public string Unidade { get; set; }

        public virtual long IdPreProposta { get; set; }
        public virtual long IdCliente { get; set; }


        ////////////////////////////
        public string Regional { get; set; }
        public string Divisao { get; set; }
        public string Produto { get; set; }
        
        public bool MatrizOferta { get; set; }


        public SimuladorDto WithRequest(DataSourceRequest request)
        {
            DataSourceRequest = request;
            return this;
        }
    }
}
