using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class PontuacaoFidelidade : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual SituacaoPontuacaoFidelidade Situacao { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual string HashDoubleCheck { get; set; }
        public virtual long IdArquivoDoubleCheck { get; set; }
        public virtual string NomeArquivoDoubleCheck { get; set; }
        public virtual string ContentTypeDoubleCheck { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual string Regional { get; set; }
        public virtual TipoPontuacaoFidelidade TipoPontuacaoFidelidade { get; set; }
        public virtual TipoCampanhaFidelidade? TipoCampanhaFidelidade { get; set; }
        public virtual long QuantidadeMinima { get; set; }
        public virtual string Codigo { get; set; }
        public virtual long Progressao { get; set; }
        public virtual string QuantidadesMinimas { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
