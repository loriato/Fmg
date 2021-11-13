using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.ApiService.Models.Login
{
    public class MensagemRetornoDto
    {
        public virtual bool Sucesso { get; set; }
        public virtual object Objeto { get; set; }
        public virtual List<string> Mensagens { get; set; }
        public virtual List<string> Campos { get; set; }

        public MensagemRetornoDto()
        {
            Sucesso = false;
            Mensagens = new List<string>();
            Campos = new List<string>();
            Objeto = null;
        }
    }
}
