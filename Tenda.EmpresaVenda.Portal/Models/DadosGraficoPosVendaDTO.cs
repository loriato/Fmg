using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Portal.Models
{
    public class DadosGraficoPosVendaDTO
    {
        public List<int> PrePropostasFinalizadas { get; set; }
        public List<string> DiasPosVendasConformidade { get; set; }
        public List<int> PosVendasConformidade { get; set; }
        public decimal PorcentagemAnterior { get; set; }
        public decimal PorcentagemAtual { get; set; }
        public int TotalKitCompleto { get; set; }
        public int TotalRepasse { get; set; }
        public int TotalConformidade { get; set; }

    }
}