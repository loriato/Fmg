using System;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class AcessoCorretorEvDTO
    {
        public string Corretor { get; set; }
        public string EmpresaVenda { get; set; }
        public DateTime InicioSessao { get; set; }
        public DateTime FimSessao { get; set; }
    }
}
