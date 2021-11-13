using System;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class AcessoDiarioRegionalEvDTO
    {
        public string Regional { get; set; }
        public string EmpresaVenda { get; set; }
        public DateTime InicioSessao { get; set; }
        public long Quantidade { get; set; }
    }
}
