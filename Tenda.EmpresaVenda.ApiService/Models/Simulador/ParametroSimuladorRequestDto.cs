using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Simulador
{
    public class ParametroSimuladorRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CorretorEmpresaVenda { get; set; }
        public string Autorizacao { get; set; }

        public string Login { get; set; }
        public string Senha { get; set; }
        public bool SouCorretor { get; set; }
        public bool ShowError { get; set; }
        public string Token { get; set; }
        public long IdPreProposta { get; set; }
        public long IdCliente { get; set; }
        public TipoOrigemSimulacao OrigemSimulacao { get; set; }
    }
}
