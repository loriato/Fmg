using System;
using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class DocumentoJuridico : BaseEntity
    {
        public virtual Arquivo Arquivo { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
