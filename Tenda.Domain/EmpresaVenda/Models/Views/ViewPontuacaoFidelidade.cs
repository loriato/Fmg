using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewPontuacaoFidelidade:BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual TipoPontuacaoFidelidade TipoPontuacaoFidelidade { get; set; }
        public virtual SituacaoPontuacaoFidelidade Situacao { get; set; }
        public virtual string Regional { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual string NomeEmpreendimento { get; set; }
        public virtual TipoCampanhaFidelidade TipoCampanhaFidelidade { get; set; }
        public virtual long IdPontuacaoFidelidade { get; set; }
        public virtual long IdEmpreendimento { get; set; }
        public virtual string Codigo { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
