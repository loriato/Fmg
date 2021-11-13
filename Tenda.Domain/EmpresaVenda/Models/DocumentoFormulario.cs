using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class DocumentoFormulario : BaseEntity
    {
        public virtual Arquivo Arquivo { get; set; }
        public virtual PreProposta PreProposta { get; set; }
        public virtual UsuarioPortal Responsavel { get; set; }
        public virtual SituacaoAprovacaoDocumento Situacao { get; set; }
        public virtual string Motivo { get; set; }
        

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
