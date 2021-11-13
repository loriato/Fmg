using System;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class AcessoDiarioEvDTO
    {
        public string EmpresaVenda { get; set; }
        public DateTime InicioSessao { get; set; }
        public long Quantidade { get; set; }
    }
}
