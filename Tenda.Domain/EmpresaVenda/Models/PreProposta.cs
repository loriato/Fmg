using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class PreProposta : BaseEntity
    {
        /// <summary>
        /// Código da pré-proposta. Gerado via Sequence
        ///  * O código de pré-proposta seguirá o formato: PPRYYMMDD00000, onde:
        /// 	** PPR: Sufixo de todas as pré-propostas
        ///      ** YY: Ano de criação da pré-proposta
        ///      ** MM: Mês de criação da pré-proposta
        ///      ** DD: Dia de criação da pré-proposta
        ///      ** 00000: Número sequencial
        /// </summary>
        public virtual string Codigo { get; set; }

        /// <summary>
        /// O dono da pré-proposta. Mesmo conceito de vendedor do SUAT
        /// Ele pode ser alterado em alguns casos por Coordenadores e Gerentes da mesma EV
        /// </summary>
        public virtual Corretor Corretor { get; set; }
        /// <summary>
        /// Mesmo conceito do Agente de Vendas no SUAT, porém dentro do contexto da Empresa de Vendas
        /// Geralmente um Coordenador ou Gerente da EV. Não pode ser alterado.
        /// </summary>
        public virtual Corretor Elaborador { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual PontoVenda PontoVenda { get; set; }
        public virtual BreveLancamento BreveLancamento { get; set; }
        public virtual UsuarioPortal Viabilizador { get; set; }
        /// <summary>
        /// Status que vai representar o HardCoded da máquina de estados
        /// </summary>
        public virtual SituacaoProposta? SituacaoProposta { get; set; }
        public virtual DateTime? DataElaboracao { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual decimal TotalDetalhamentoFinanceiro { get; set; }
        public virtual decimal TotalItbiEmolumento { get; set; }
        public virtual DateTime? NascimentoMaisVelho { get; set; }
        public virtual decimal RendaBrutaFamiliar { get; set; }
        public virtual decimal FgtsFamiliar { get; set; }
        public virtual bool? ClienteCotista { get; set; }
        public virtual bool? DocCompleta { get; set; }
        public virtual bool? FatorSocial { get; set; }
        public virtual bool? FaixaUmMeio { get; set; }
        public virtual StatusSicaq StatusSicaq { get; set; }
        public virtual DateTime? DataSicaq { get; set; }
        public virtual long IdSuat { get; set; }
        public virtual long IdUnidadeSuat { get; set; }
        public virtual string IdentificadorUnidadeSuat { get; set; }
        public virtual string Observacao { get; set; }
        public virtual long IdOrigem { get; set; }
        public virtual long IdTorre { get; set; }
        public virtual string ObservacaoTorre { get; set; }
        public virtual string NomeTorre { get; set; }
        public virtual decimal ParcelaAprovada { get; set; }
        public virtual decimal ParcelaSolicitada { get; set; }
        public virtual string PassoAtualSuat { get; set; }
        public virtual Avalista Avalista { get; set; }
        public virtual TipoOrigemCliente OrigemCliente { get; set; }
        //public virtual UsuarioGrupoCCA UltimoCCA { get; set; }
        public virtual string UltimoCCA { get; set; }

        //public virtual bool IsBreveLancamento { get; set; }
        //public virtual EstadoCidade Regiao { get; set; }
        public virtual bool FaixaEv { get; set; }

        public virtual string CodigoSimulacao { get; set; }
        public virtual string DigitoSimulacao { get; set; }

        //Sicaq Prévio
        public virtual bool? FaixaUmMeioPrevio { get; set; }
        public virtual StatusSicaq StatusSicaqPrevio { get; set; }
        public virtual DateTime? DataSicaqPrevio { get; set; }
        public virtual decimal ParcelaAprovadaPrevio { get; set; }
        public virtual long ContadorSicaq { get; set; }
        public virtual string JustificativaReenvio { get; set; }
        public virtual string CpfIndicador { get; set; }
        public virtual string NomeIndicador { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
