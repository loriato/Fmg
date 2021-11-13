using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewValorNominalEmpreendimento : BaseEntity
    {
        public virtual string NomeEmpreendimento { get; set; }
        public virtual string Divisao { get; set; }
        public virtual string Estado { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual decimal FaixaUmMeioDe { get; set; }
        public virtual decimal FaixaUmMeioAte { get; set; }
        public virtual decimal FaixaDoisDe { get; set; }
        public virtual decimal FaixaDoisAte { get; set; }
        public virtual decimal PNEDe { get; set; }
        public virtual decimal PNEAte { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual SituacaoValorNominal Situacao { get; set; }


        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
