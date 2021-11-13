using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class SupervisorViabilizador : BaseEntity
    {
        public virtual Usuario Supervisor { get; set; }
        public virtual Usuario Viabilizador { get; set; } 
        public override string ChaveCandidata()
        {
            return Supervisor.Nome +" x "+ Viabilizador.Nome;
        }
    }
}
