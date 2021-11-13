using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class RegraComissaoEvs : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual SituacaoRegraComissao Situacao { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual string HashDoubleCheck { get; set; }
        public virtual long IdArquivoDoubleCheck { get; set; }
        public virtual string NomeDoubleCheck { get; set; }
        public virtual string ContentTypeDoubleCheck { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual string Regional { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual RegraComissao RegraComissao { get; set; }
        public virtual TipoRegraComissao Tipo { get; set; }
        public virtual string Codigo { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
