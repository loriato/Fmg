using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{

    public class ViewPreProposta : BaseEntity
    {

        public virtual string Codigo { get; set; }
        public virtual DateTime Elaboracao { get; set; }
        public virtual long IdCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual string CpfCnpj { get; set; }
        public virtual DateTime? Nascimento { get; set; }
        public virtual TipoRenda? TipoRenda { get; set; }
        public virtual decimal RendaApurada { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string RazaoSocialEmpresaVenda { get; set; }
        public virtual bool ClienteCotista { get; set; }
        public virtual bool DocCompleta { get; set; }
        public virtual decimal FgtsApurado { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual long IdElaborador { get; set; }
        public virtual string NomeElaborador { get; set; }
        public virtual bool DetalhamentoFinanceiroPreenchido { get; set; }
        public virtual bool FatorSocial { get; set; }
        public virtual long IdBreveLancamento { get; set; }
        public virtual string NomeBreveLancamento { get; set; }
        public virtual string Regional { get; set; }

        public virtual string UF { get; set; }
        public virtual long IdPontoVenda { get; set; }
        public virtual string NomePontoVenda { get; set; }
        public virtual decimal Entrada { get; set; }
        public virtual decimal PreChaves { get; set; }
        public virtual decimal PreChavesIntermediaria { get; set; }
        public virtual decimal Fgts { get; set; }
        public virtual decimal Subsidio { get; set; }
        public virtual decimal Financiamento { get; set; }
        public virtual decimal PosChaves { get; set; }
        public virtual DateTime? DataEnvio { get; set; }
        public virtual int NumeroDocumentosPendentes { get; set; }
        public virtual int IdAnalistaSicaq { get; set; }
        public virtual string NomeAnalistaSicaq { get; set; }
        public virtual DateTime? DataSicaq { get; set; }
        public virtual int IdAssistenteAnalise { get; set; }
        public virtual string NomeAssistenteAnalise { get; set; }
        public virtual string MotivoParecer { get; set; }
        public virtual string MotivoPendencia { get; set; }
        public virtual StatusSicaq? StatusSicaq { get; set; }
        public virtual SituacaoProposta StatusAnalise { get; set; }
        public virtual long IdViabilizador { get; set; }
        public virtual string NomeViabilizador { get; set; }
        public virtual string SituacaoPrePropostaSuatEvs { get; set; }
        public virtual long IdStatusPreProposta { get; set; }
        public virtual string SituacaoPrePropostaPortalHouse { get; set; }
        public virtual string SituacaoPrePropostaHouse { get; set; }
        public virtual long IdAgrupamentoPrePropostaHouse { get; set; }
        public virtual string CodigoSistemaHouse { get; set; }
        public virtual string SituacaoPrePropostaCorretor { get; set; }
        public virtual long IdAgrupamentoPrePropostaCorretor { get; set; }
        public virtual string CodigoSistemaCorretor { get; set; }
        public virtual string SituacaoPrePropostaLoja { get; set; }
        public virtual long IdAgrupamentoPrePropostaLoja { get; set; }
        public virtual string CodigoSistemaLoja { get; set; }
        public virtual decimal ParcelaAprovada { get; set; }
        public virtual decimal ParcelaAprovadaPrevio { get; set; }
        public virtual TipoOrigemCliente OrigemCliente { get; set; }
        public virtual long IdStandVenda { get; set; }
        public virtual string NomeStandVenda { get; set; }
        public virtual long IdProposta { get; set; }
        public virtual string CodigoProposta { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual DateTime? DataFaturado { get; set; }
        public virtual long IdSuat { get; set; }
        public virtual string NomeCCA { get; set; }
        public virtual long ContadorSicaq { get; set; }
        public virtual TipoEmpresaVenda TipoEmpresaVenda { get; set; }
        public virtual SituacaoAvalista SituacaoAvalista { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual DateTime? DataSicaqPrevio { get; set; }
        public virtual StatusSicaq? StatusSicaqPrevio { get; set; }
        public virtual string NomeTorre { get; set; }
        public virtual string DescricaoTorre { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}