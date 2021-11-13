using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class RegionalEmpresa : BaseEntity
    {
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual Regionais Regional { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
