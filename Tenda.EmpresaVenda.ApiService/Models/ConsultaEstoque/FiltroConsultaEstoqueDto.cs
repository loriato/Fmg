using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.ConsultaEstoque
{
    public class FiltroConsultaEstoqueDto : FilterDto
    {
        public string Divisao { get; set; }
        public string Caracteristicas { get; set; }
        public DateTime PrevisaoEntrega { get; set; }
        public long IdTorre { get; set; }

    }
}
