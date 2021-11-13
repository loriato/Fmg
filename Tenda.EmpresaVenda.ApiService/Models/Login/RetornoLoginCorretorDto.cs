using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.Login
{
    public class RetornoLoginCorretorDto
    {
        public UsuarioPortal UsuarioPortal { get; set; }
        public Acesso Acesso { get; set; }
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda EmpresaVenda { get; set; }
        public string Hash { get; set; }
    }
}
