using Europa.Data.Model;
using Tenda.Domain.Security.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.Security.Models
{
    public class LogExecucao : BaseEntity
    {
        public virtual DateTime DataCriacao { get { return CriadoEm; } }
        public virtual string Log { get; set; }
        public virtual TipoLog Tipo { get; set; }
        public virtual Execucao Execucao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
