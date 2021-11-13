using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPrePropostaAguardandoAnalise : BaseEntity
    {
        public virtual string UF { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual string ProponenteUm { get; set; }
        public virtual string CpfProponenteUm { get; set; }
        public virtual string ProponenteDois { get; set; }
        public virtual string CpfProponenteDois { get; set; }
        public virtual string BreveLancamento { get; set; }
        public virtual string EmpresaVenda { get; set; }
        public virtual string AgenteViabilizador { get; set; }
        public virtual string NomeElaborador { get; set; }
        public virtual string Corretor { get; set; }
        public virtual DateTime? DataElaboracao { get; set; }
        public virtual TimeSpan? HoraElaboracao { get; set; }
        public virtual DateTime? DataEnvioPreProposta { get; set; }
        public virtual TimeSpan? HoraEnvioPreProposta { get; set; }
        public virtual DateTime? DataInicioAnalise { get; set; }
        public virtual TimeSpan? HoraInicioAnalise { get; set; }
        public virtual DateTime? DataHoraUltimoEnvio { get; set; }
        public virtual TimeSpan? HoraFimAnalise { get; set; }
        public virtual long IdAssistenteAnalise { get; set; }
        public virtual string NomeAssistenteAnalise { get; set; }
        public virtual int Contador { get; set; }
        public virtual int ContadorTotal { get; set; }
        public virtual bool PropostasAnteriores { get; set; }
        public virtual int NumeroPropostasAnteriores { get; set; }
        public virtual string MotivoPendencia { get; set; }
        public virtual string MotivoParecer { get; set; }
        public virtual SituacaoProposta StatusPreProposta { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual long IdProponenteUm { get; set; }
        public virtual long IdProponenteDois { get; set; }
        public virtual long IdBreveLancamento { get; set; }
        public virtual long IdCorretor { get; set; }
        public virtual long IdEnvioPreProposta { get; set; }
        public virtual long IdAnalisePreProposta { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdAnaliseSicaq { get; set; }
        public virtual long IdAnalistaSicaq { get; set; }
        public virtual long IdViabilizador { get; set; }
        public virtual DateTime? DataSicaq { get; set; }
        public virtual TimeSpan? HoraSicaq { get; set; }
        public virtual string NomeAnalistaSicaq { get; set; }
        public virtual StatusSicaq StatusSicaq { get; set; }
        public virtual string SituacaoPrePropostaSuatEvs { get; set; }
        public virtual decimal ParcelaAprovada { get; set; }

        public virtual int ContadorPrimeiraAnalise { get; set; }
        public virtual int QuantidadeAnalise { get; set; }
        public virtual DateTime? DataInicioPrimeiraAnalise { get; set; }
        public virtual DateTime? DataTerminoPrimeiraAnalise { get; set; }
        public virtual TimeSpan? HoraFimPrimeiraAnalise { get; set; }
        public virtual TimeSpan? HoraInicioPrimeiraAnalise { get; set; }
        public virtual string NomeUsuarioPrimeiraAnalise { get; set; }

        public virtual DateTime? DataInicioUltimaAnalise { get; set; }
        public virtual DateTime? DataTerminoUltimaAnalise { get; set; }
        public virtual TimeSpan? HoraFimUltimaAnalise { get; set; }
        public virtual TimeSpan? HoraInicioUltimaAnalise { get; set; }
        public virtual string NomeUsuarioUltimaAnalise { get; set; }
        public virtual string Observacao { get; set; }
        public virtual long IdAvalista { get; set; }
        public virtual SituacaoAprovacaoDocumento SituacaoDocumento { get; set; }
        public virtual SituacaoAvalista? SituacaoAvalista { get; set; }

        //Sicaq Prévio
        public virtual bool? FaixaUmMeioPrevio { get; set; }
        public virtual StatusSicaq StatusSicaqPrevio { get; set; }
        public virtual DateTime? DataSicaqPrevio { get; set; }
        public virtual decimal ParcelaAprovadaPrevio { get; set; }
        public virtual long IdCCAOrigem { get; set; }
        public virtual long IdCCADestino { get; set; }

        public virtual long IdRegional { get; set; }
        public virtual string Regional { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
