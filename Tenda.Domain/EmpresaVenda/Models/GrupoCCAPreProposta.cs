using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class GrupoCCAPreProposta : BaseEntity
    {
        public virtual GrupoCCA GrupoCCAOrigem { get; set; }

        public virtual GrupoCCA GrupoCCADestino { get; set; }

        public virtual long IdPreProposta { get; set; }

        public virtual Situacao Situacao { get; set; }

        public GrupoCCAPreProposta()
        {
            GrupoCCAOrigem = new GrupoCCA();
            GrupoCCADestino = new GrupoCCA();
        }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
