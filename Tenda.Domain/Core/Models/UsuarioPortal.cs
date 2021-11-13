using System;
using System.ComponentModel.DataAnnotations;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Core.Models
{
    public class UsuarioPortal : Usuario
    {
        [Required]
        public virtual bool IgnoraVerificacaoCadastral { get; set; }
        public virtual DateTime? UltimaLeituraNotificacao { get; set; }

        public UsuarioPortal()
        {
        }

        public UsuarioPortal(Usuario usuario)
        {
            Email = usuario.Email;
            Id = usuario.Id;
            Login = usuario.Login;
            Nome = usuario.Nome;
            Senha = usuario.Senha;
            Situacao = usuario.Situacao;
        }

    }
}