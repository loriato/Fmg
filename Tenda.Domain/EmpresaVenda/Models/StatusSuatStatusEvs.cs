using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class StatusSuatStatusEvs : BaseEntity
    {

        public virtual string DescricaoSuat { get; set; }
        public virtual string DescricaoEvs { get; set; }

        public override string ChaveCandidata()
        {
            return DescricaoSuat + " - " + DescricaoEvs;
        }
    }
}
