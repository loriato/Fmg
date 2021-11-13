using Europa.Data.Model;
using System;

namespace Tenda.Domain.Security.Views
{
    public class ViewLogExecucao : BaseEntity
    {
        public virtual long IdQuartzConfiguration { get; set; }
        public virtual DateTime DataInicioExecucao { get; set; }
        public virtual DateTime? DataTerminoExecucao { get; set; }
        public virtual string Log { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
