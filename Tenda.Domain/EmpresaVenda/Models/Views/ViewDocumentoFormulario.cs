using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewDocumentoFormulario : BaseEntity
    {
        public virtual long IdArquivo { get; set; }
        public virtual string NomeDocumento { get; set; }
        public virtual long IdPreProposta { get; set; }
        public virtual string CodigoPreProposta { get; set; }
        public virtual long IdResponsavel { get; set; }
        public virtual string NomeResponsavel { get; set; }
        public virtual SituacaoAprovacaoDocumento Situacao { get; set; }
        public virtual string Motivo { get; set; }
        public virtual SituacaoProposta SituacaoPreProposta { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
