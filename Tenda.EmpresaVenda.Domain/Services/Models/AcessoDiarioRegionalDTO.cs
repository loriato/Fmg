using System;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class AcessoDiarioRegionalDTO
    {
        public string Regional { get; set; }
        public DateTime InicioSessao { get; set; }
        public long Quantidade { get; set; }
    }
}
