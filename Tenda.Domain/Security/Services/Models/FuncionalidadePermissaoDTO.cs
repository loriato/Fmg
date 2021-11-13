using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Services.Models
{
    public class FuncionalidadePermissaoDTO
    {
        public Funcionalidade Funcionalidade { get; set; }
        public bool Permitido { get; set; }
        public bool Logar { get; set; }
    }
}
