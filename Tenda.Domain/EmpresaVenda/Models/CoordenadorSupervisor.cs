using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class CoordenadorSupervisor : BaseEntity
    {
        public virtual Usuario Supervisor { get; set; }
        public virtual Usuario Coordenador { get; set; }
        public override string ChaveCandidata()
        {
            return Supervisor.Nome + " x " + Coordenador.Nome;
        }
    }
}
