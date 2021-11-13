using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class PosVendaGraficoFiltro
    {
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public long IdEmpresaVenda { get; set; }
        public SituacaoContrato SituacaoContrato { get; set; }
    }
}
