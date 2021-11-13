using Europa.Data.Model;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class CoordenadorViabilizador:BaseEntity
    {
        public virtual Usuario Viabilizador { get; set; }
        public virtual Usuario Coordenador { get; set; }
        public override string ChaveCandidata()
        {
            return Viabilizador.Nome + " x " + Coordenador.Nome;
        }
    }
}
