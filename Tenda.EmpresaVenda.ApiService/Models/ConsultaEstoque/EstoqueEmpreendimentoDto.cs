using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tenda.EmpresaVenda.ApiService.Models.ConsultaEstoque
{
    public class EstoqueEmpreendimentoDto
    {
        public long IdEmpreendimento { get; set; }
        public string NomeEmpreendimento { get; set; }
        public string Divisao { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public decimal QtdeDisponivel { get; set; }
        public decimal QtdeReservado { get; set; }
        public decimal QtdeVendido { get; set; }
        public string Caracteristicas { get; set; }
        public decimal QtdeUnidades { get; set; }
        public decimal MenorM2 { get; set; }
        public decimal MaiorM2 { get; set; }
        public DateTime PrevisaoEntrega { get; set; }
        public long IdRegional { get; set; }
        public string TipologiaUnidade { get; set; }

    }
}
