using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class FiltroStandVendaDTO
    {
        public virtual string Nome { get; set; }
        public virtual string Estado { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual long IdPontoVenda { get; set; }
        public virtual string RegionalEV { get; set; }
        public virtual long IdStandVenda { get; set; }
    }
}
