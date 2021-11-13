using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.Domain.Security.Models
{
    public class Execucao : BaseEntity
    {
        public virtual QuartzConfiguration Quartz { get; set; }
        public virtual DateTime DataInicioExecucao { get; set; }
        public virtual DateTime DataFimExecucao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
