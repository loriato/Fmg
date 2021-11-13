using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class TipoCusto : BaseEntity
    {
        public virtual string Descricao { get; set; }

        public override string ChaveCandidata()
        {
            return Descricao;
        }
    }
}
