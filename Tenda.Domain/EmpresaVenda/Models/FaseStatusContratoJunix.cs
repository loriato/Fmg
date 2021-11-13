using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class FaseStatusContratoJunix : BaseEntity
    {
        public virtual string Fase { get; set; }
        public override string ChaveCandidata()
        {
            return Fase;
        }
    }
}
