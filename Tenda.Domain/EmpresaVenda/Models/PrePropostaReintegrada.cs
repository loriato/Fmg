using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class PrePropostaReintegrada : BaseEntity
    {
        public virtual long IdSuat { get; set; }
        public virtual long IdUnidadeSuat { get; set; }
        public virtual PreProposta PreProposta { get; set; }
        public virtual string IdentificadorUnidadeSuat { get; set; }
        public virtual long IdTorre { get; set; }
        public virtual string ObservacaoTorre { get; set; }
        public virtual string NomeTorre { get; set; }
        public virtual string PassoAtualSuat { get; set; }
        public virtual long IdBreveLancamento { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
