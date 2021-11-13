using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class DocumentoAvalista : BaseEntity
    {
        public virtual TipoDocumentoAvalista TipoDocumento { get; set; }
        public virtual Avalista Avalista{get; set;}
        public virtual Arquivo Arquivo { get; set; }
        public virtual PreProposta PreProposta { get; set; }
        public virtual SituacaoAprovacaoDocumentoAvalista Situacao { get; set; }
        public virtual string Motivo { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
