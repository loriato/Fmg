using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class AlteracaoSenhaCorretorDTO
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string TokenAtivacao { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }
    }
}