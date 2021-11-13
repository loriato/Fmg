using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ParecerDocumentoAvalista : BaseEntity
    {
        public virtual DocumentoAvalista DocumentoAvalista { get; set; }
        public virtual string Parecer { get; set; }
        public virtual DateTime? Validade { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
