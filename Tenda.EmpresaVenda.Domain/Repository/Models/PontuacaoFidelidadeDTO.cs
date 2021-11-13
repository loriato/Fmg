using Europa.Data.Model;
using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class PontuacaoFidelidadeDTO : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual SituacaoPontuacaoFidelidade? Situacao { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual string HashDoubleCheck { get; set; }
        public virtual long IdArquivoDoubleCheck { get; set; }
        public virtual string NomeArquivoDoubleCheck { get; set; }
        public virtual string ContentTypeDoubleCheck { get; set; }
        public virtual string Regional { get; set; }
        public virtual TipoPontuacaoFidelidade? TipoPontuacaoFidelidade { get; set; }
        public virtual TipoCampanhaFidelidade? TipoCampanhaFidelidade { get; set; }
        public virtual long QuantidadeMinima { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string Codigo { get; set; }

        public bool? Create{ get; set; }
        public  List<long> IdEmpreendimentos{ get; set; } 
        public  List<long> IdEmpresasVenda{ get; set; } 
        public  long? IdPontuacaoFidelidade{ get; set; }        
        public bool Ultimo { get; set; }

        public virtual decimal PontuacaoTotal { get; set; }
        public virtual decimal PontuacaoIndisponivel { get; set; }
        public virtual decimal PontuacaoDisponivel { get; set; }
        public virtual decimal PontuacaoResgatada { get; set; }
        public virtual decimal PontuacaoSolicitada { get; set; }
        public virtual long IdSolicitadoPor { get; set; }
        public DateTime? VigenteEm { get; set; }

        public virtual SituacaoResgate? SituacaoResgate { get; set; }
        public virtual List<long> IdsEmpresaVenda { get; set; }
        public virtual DateTime? PeriodoDe { get; set; }
        public virtual DateTime? PeriodoAte { get; set; }
        public virtual string Voucher { get; set; }
        public virtual string Motivo { get; set; }
        public virtual long IdResgate { get; set; }
        
        public virtual bool DataSolicitacao { get; set; }
        public virtual bool DataLiberacao { get; set; }

        public virtual long Progressao { get; set; }
        public virtual string QuantidadesMinimas { get; set; }

        public PontuacaoFidelidadeDTO()
        {
            QuantidadesMinimas = "[]";
        }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
