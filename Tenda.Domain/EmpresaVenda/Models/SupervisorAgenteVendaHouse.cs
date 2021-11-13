using System;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class SupervisorAgenteVendaHouse : BaseHierarquiaHouse
    {
        public virtual UsuarioPortal AgenteVenda { get; set; }
        public virtual UsuarioPortal Supervisor { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
