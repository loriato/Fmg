using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCCAPreProposta : BaseEntity
    {
        public virtual long IdPreProposta { get; set; }
        public virtual long IdGrupoCCA { get; set; }
        public virtual string NomeGrupoCCA { get; set; }
        public virtual long IdEmpresaVenda { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
