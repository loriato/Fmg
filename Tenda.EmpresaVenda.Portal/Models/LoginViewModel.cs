using System;
using System.ComponentModel.DataAnnotations;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo 'Usuário' é obrigatório")]
        public String Username { get; set; }

        [Required(ErrorMessage = "O campo 'Senha' é obrigatório")]
        public String Password { get; set; }

        public String ReturnUrl { get; set; }
    }
}