using System;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class CoordenadorSupervisorHouse : BaseHierarquiaHouse
    {
        public virtual UsuarioPortal Coordenador { get; set; }
        public virtual UsuarioPortal Supervisor { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
