
using Tenda.Domain.Core.Models;

namespace Tenda.EmpresaVenda.Web.Models
{
    public class UsuarioViewModel
    {
        public UsuarioPortal Usuario { get; set; }
        public long PerfilId { get; set; }
        public string NomePerfil { get; set; }
    }
}