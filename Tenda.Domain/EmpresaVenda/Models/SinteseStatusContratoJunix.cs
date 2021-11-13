using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class SinteseStatusContratoJunix : BaseEntity
    {
        public virtual string Sintese { get; set; }
        public virtual string StatusContrato { get; set; }
        public virtual FaseStatusContratoJunix FaseJunix { get; set; }
        public override string ChaveCandidata()
        {
            return Sintese;
        }
    }
}
