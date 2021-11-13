using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
     public class OcorrenciasMidas : BaseEntity
    {
        public virtual long OccurenceId { get; set; }
        public virtual string TaxIdTaker { get; set; }
        public virtual string TaxIdProvider { get; set; }

        public virtual bool CanBeCommissioned { get; set; }
        public virtual DocumentoMidas Document { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }

        public OcorrenciasMidas()
        {
            CanBeCommissioned = false;
            Document = new DocumentoMidas();
        }
    }
}
