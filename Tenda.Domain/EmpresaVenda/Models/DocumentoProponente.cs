using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class DocumentoProponente : BaseEntity
    {
        public virtual TipoDocumento TipoDocumento { get; set; }
        public virtual Proponente Proponente { get; set; }
        public virtual PreProposta PreProposta { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual SituacaoAprovacaoDocumento Situacao { get; set; }
        public virtual string Motivo { get; set; }
        public virtual DateTime? DataExpiracao { get; set; }
        
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}