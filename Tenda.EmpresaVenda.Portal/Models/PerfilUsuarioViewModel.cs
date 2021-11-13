using Tenda.Domain.Core.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class PerfilUsuarioViewModel
    {
        public long IdUsuario { get; set; }
        public bool IsOperador { get; set; }
        public bool IsVendedor { get; set; }
    }
}