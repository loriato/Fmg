using System;

namespace Tenda.EmpresaVenda.ApiService.Models.ConsultaEstoque
{
    public class EstoqueUnidadeDto
    {
        public long IdEmpreendimento { get; set; }
        public string NomeEmpreendimento { get; set; }
        public string Divisao { get; set; }
        public long IdTorre { get; set; }
        public string NomeTorre { get; set; }
        public long IdUnidade { get; set; }
        public string IdSapTorre { get; set; }
        public string NomeUnidade { get; set; }
        public string Caracteristicas { get; set; }
        public decimal Metragem { get; set; }
        public string Andar { get; set; }
        public string Prumada { get; set; }
        public string IdSapUnidade { get; set; }
        public DateTime? DataEntregaObra { get; set; }
        public string TipologiaUnidade { get; set; }
        public bool Disponivel { get; set; }
        public bool Reservada { get; set; }
        public bool Vendida { get; set; }

    }
}
