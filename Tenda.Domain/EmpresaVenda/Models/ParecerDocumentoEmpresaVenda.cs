using Europa.Data.Model;
using System;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ParecerDocumentoEmpresaVenda : BaseEntity
    {
        public virtual DocumentoEmpresaVenda DocumentoEmpresaVenda { get; set; }
        public virtual string Parecer { get; set; }
        public virtual DateTime? Validade { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
