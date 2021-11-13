using System.Collections.Generic;

namespace Tenda.EmpresaVenda.ApiService.Models.Login
{
    public class LoginPermissaoDto
    {
        public string UnidadeFuncional { get; set; }
        public List<string> Funcionalidades { get; set; }
    }
}
