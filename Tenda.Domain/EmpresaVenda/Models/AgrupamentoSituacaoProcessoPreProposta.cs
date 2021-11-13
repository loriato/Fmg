using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class AgrupamentoSituacaoProcessoPreProposta : BaseEntity
    {
        public virtual Sistema Sistema { get; set; }
        public virtual StatusPreProposta StatusPreProposta { get; set; }
        public virtual AgrupamentoProcessoPreProposta Agrupamento { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
