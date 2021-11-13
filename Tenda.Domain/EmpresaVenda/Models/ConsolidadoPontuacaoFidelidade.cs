using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ConsolidadoPontuacaoFidelidade : BaseEntity
    {
        public virtual long IdProposta { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual long IdItemPontuacaoFidelidade { get; set; }
        public virtual decimal Pontuacao { get; set; }
        public virtual DateTime DataPontuacao { get; set; }
        public virtual SituacaoPontuacao SituacaoPontuacao { get; set; }
        public virtual bool Faturado { get; set; }
        public virtual long IdPontuacaoFidelidade { get; set; }
        public virtual Tipologia Tipologia { get; set; }
        public virtual long IdValorNominal { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
