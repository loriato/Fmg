using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ParecerDocumentoProponente : BaseEntity
    {
        public virtual DocumentoProponente DocumentoProponente { get; set; }
        public virtual string Parecer { get; set; }
        public virtual DateTime? Validade { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
