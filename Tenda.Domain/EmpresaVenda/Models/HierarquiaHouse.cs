using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class HierarquiaHouse : BaseHierarquiaHouse
    {
        public virtual EmpresaVenda House { get; set; }
        public virtual UsuarioPortal AgenteVenda { get; set; }
        public virtual UsuarioPortal Supervisor { get; set; }
        public virtual UsuarioPortal Coordenador { get; set; }
    }
}
